using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.Emit;
using System.Threading;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {

            ExamineAnAssembly();
            ExamineAnAssembly2();
            GetAssemblyAttributes();
            GetAssemblyAttributes2();
            AssemblyGetTypes();
            ObjectGetType();
            GetInfoFromType();
            GetInfoFromAnimals();
            EnumerateMethods();
            GetNestedTypes();
            GetMembers();
            GetMethodBody();
            FunWithBindingFlags();

            LoadObjectDynamically();
            StaticMethodInvocation();

            CreateDynamicAssembly();
            Console.ReadKey();
        }

        private static void CreateDynamicAssembly()
        {
            AssemblyName asName = new AssemblyName();
            asName.Name = "MyNewAssembly";
            
            
            AssemblyBuilder myAsBuilder = 
                AppDomain.CurrentDomain.DefineDynamicAssembly(asName, AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder myModBuilder = myAsBuilder.DefineDynamicModule("MyNewAssembly");
            
            TypeBuilder myTypeBuilder =
                myModBuilder.DefineType("MyNewType", TypeAttributes.Public | TypeAttributes.Class);

            //create a default constructor
            ConstructorBuilder myConBuilder = myTypeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            //create a secondary constructor
            ConstructorBuilder myConBuilder2 = myTypeBuilder.DefineConstructor(MethodAttributes.Public,
                CallingConventions.Standard, new Type[] { typeof(int) });

            ILGenerator gen = myConBuilder2.GetILGenerator();
            gen.EmitWriteLine("secondary constructor was called");
            gen.Emit(OpCodes.Ret);

            //create a method
            MethodBuilder myMethodBuilder = myTypeBuilder.DefineMethod("Add", MethodAttributes.Public,
                null, new Type[] { typeof(string) });

            //create a static method
            MethodBuilder myStaticMethodBuilder = myTypeBuilder.DefineMethod("StaticAdd",
                MethodAttributes.Public | MethodAttributes.Static,
                null,
                new Type[] { typeof(string) });

            //create a private field
            FieldBuilder myFieldBuilder = myTypeBuilder.DefineField("_count",
                typeof(int),
            FieldAttributes.Private);

            //create a property
            PropertyBuilder myPropertyBuilder = myTypeBuilder.DefineProperty("Count",
                PropertyAttributes.None,
                typeof(int),
                Type.EmptyTypes);

            //get operation
            MethodAttributes getAttributes = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            MethodBuilder propGetBuilder = myTypeBuilder.DefineMethod("get_Count",
                getAttributes, typeof(int), Type.EmptyTypes);

            //add the method as the get operation on the property
            myPropertyBuilder.SetGetMethod(propGetBuilder);

            //persist the assembly to the disk
            myAsBuilder.Save("MyNewAssembly.dll");

            
        }

        private static void StaticMethodInvocation()
        {
            Type t = typeof(System.Console);
            MethodInfo m = t.GetMethod("WriteLine",new Type[] {typeof(string)});
            m.Invoke(null, new object[] { "Hello World!" });
        }

        private static void LoadObjectDynamically()
        {
            Assembly a = Assembly.LoadFile(Environment.CurrentDirectory + @"\DynamicLibrary.dll");
            Type t = a.GetType("DynamicLibrary.Product");
            object o1 = Activator.CreateInstance(t);
            object o2 = Activator.CreateInstance(t, new object[] {"MyProduct"});

            Type[] argumentTypes = Type.EmptyTypes;
            ConstructorInfo ctor = t.GetConstructor(argumentTypes);
            object o3 = ctor.Invoke(new object[] {});

            Type[] argumentTypes2 = new Type[] {typeof(string)};
            ConstructorInfo ctor2 = t.GetConstructor(argumentTypes2);
            object o4 = ctor2.Invoke(new object[] { "MyProduct2" });

            //play with property
            PropertyInfo p = o1.GetType().GetProperty("Name");
            p.SetValue(o1,"ReflectedProduct",null);
            Console.WriteLine(p.GetValue(o1,null));

            //play with event
            //get event info
            EventInfo e = o2.GetType().GetEvent("PriceChanged");
            //create an event handler
            EventHandler handler = new EventHandler(Product_PriceChanged);
            //register for the event
            e.AddEventHandler(o2,handler);
            //modify the Price property in order to raise the event
            o2.GetType().GetProperty("Price").SetValue(o2, (float)50.5, null);

        }

        private static void Product_PriceChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Price Changed");

            //get the new price
            Type t = sender.GetType();
            Console.WriteLine("New Price is {0}",t.GetProperty("Price").GetValue(sender,null));

            //get methods WritePrice and WriteName
            MethodInfo mi = t.GetMethod("WritePrice", BindingFlags.Instance |
                                                      BindingFlags.NonPublic);
            MethodInfo mi2 = t.GetMethod("WriteName");
            mi.Invoke(sender, null);
            mi2.Invoke(sender, null);
        }

        private static void FunWithBindingFlags()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType("Reflection.MyClass");
            MemberInfo[] m = t.GetMembers(BindingFlags.Public | BindingFlags.Instance);
            foreach(MemberInfo me in m)
                Console.WriteLine("Member {0}",me.Name);
            Console.WriteLine();
        }

        private static void GetMethodBody()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType("Reflection.MyClass");
            MethodInfo m = t.GetMethod("Write");
            MethodBody mb = m.GetMethodBody();
            
            foreach(LocalVariableInfo l in mb.LocalVariables)
                Console.WriteLine("Local Variable {0} {1}",l.LocalType,l.LocalIndex);

            foreach(Byte b in mb.GetILAsByteArray())
                Console.WriteLine("{0:x2}",b);
            Console.WriteLine();
        }

        private static void GetMembers()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType("Reflection.MyClass");
            foreach (MemberInfo m in t.GetMembers())
            {
                Console.WriteLine(" {0} {1}", m.MemberType,m.Name);

                if(m.MemberType == MemberTypes.Event)
                {
                    EventInfo e = m as EventInfo;
                    if (e != null) Console.WriteLine("Found an event with name {0}",e.Name);
                }
            }
            Console.WriteLine();
        }

        private static void GetNestedTypes()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType("Reflection.MyClass");
            foreach(Type nestedType in t.GetNestedTypes())
            {
                Console.WriteLine("Nested Type = {0}",nestedType.Name);
            }
            Console.WriteLine();
        }

        private static void EnumerateMethods()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType("Reflection.MyClass");
            foreach(MethodInfo m in t.GetMethods())
            {
                Console.WriteLine("Method name is {0}",m.Name);
                Console.WriteLine("Return type is {0}",m.ReturnParameter.Name);
                foreach(ParameterInfo p in m.GetParameters())
                {
                    Console.WriteLine("Parameter name = {0}",p.Name);
                }
            }
            Console.WriteLine();
        }

        private static void GetInfoFromAnimals()
        {
            Animal a  = new Animal();
            Animal d = new Dog();
            Animal c = new Cat();

            Console.WriteLine("Type for a is {0}", a.GetType().Name);
            Console.WriteLine("Type for d is {0}", d.GetType().Name);
            Console.WriteLine("Type for c is {0}", c.GetType().Name);
            Console.WriteLine();
        }

        private static void GetInfoFromType()
        {
            Type t = typeof (double);
            Console.WriteLine("Namespace : {0}",t.Namespace);
            Console.WriteLine("Fullname : {0}",t.FullName);
            Console.WriteLine("Is ValueType : {0}",t.IsValueType);
            Console.WriteLine("Is Sealed : {0}",t.IsSealed);
            Console.WriteLine("Is Serializable : {0}",t.IsSerializable);
            Console.WriteLine("Is Public : {0}",t.IsPublic);
            Console.WriteLine("Is Primitive : {0}",t.IsPrimitive);
            Console.WriteLine("Is Class : {0}",t.IsClass);

            foreach(Attribute Atr in t.GetCustomAttributes(false))
                Console.WriteLine(" {0}",Atr.GetType().Name);
            Console.WriteLine();
        }

        private static void ObjectGetType()
        {
            MyClass x = new MyClass();
            Type t = x.GetType();

            Type t2 = typeof (MyClass);
            Type t3 = typeof (int);
            Console.WriteLine();
        }



        private static void AssemblyGetTypes()
        {
            Assembly a  = Assembly.GetExecutingAssembly();
            Type[] aTypes = a.GetTypes();

            Module m = a.GetModules()[0];
            Type[] mTypes = m.GetTypes();
            Console.WriteLine();
        }

        private static void GetAssemblyAttributes()
        {
            object[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(false);
            foreach (Attribute attr in attrs)
            {
                Console.WriteLine("Attribute {0} ",attr.GetType().Name);
            }
            Console.WriteLine();
        }

        private static void GetAssemblyAttributes2()
        {
            Type attrType = typeof (AssemblyFileVersionAttribute);
            object[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(attrType, false);
            if(attrs.Length>0)
            {
                AssemblyFileVersionAttribute version = attrs[0] as AssemblyFileVersionAttribute;
                if(version != null)
                {
                    Console.WriteLine("File Version is: {0}",version.Version);
                }
            }
            Console.WriteLine();
        }


       
        private static void ExamineAnAssembly()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Console.WriteLine(a.CodeBase);
            Console.WriteLine(a.EntryPoint);
            Console.WriteLine(a.FullName);
            Console.WriteLine(a.GlobalAssemblyCache.ToString());
            Console.WriteLine(a.Location);
            Console.WriteLine(a.ImageRuntimeVersion);
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void ExamineAnAssembly2()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Module[] modules = a.GetModules();
            foreach (Module m in modules)
            {
                Console.WriteLine(m.Assembly);
                Console.WriteLine(m.FullyQualifiedName);
                Console.WriteLine(m.Name);
                Console.WriteLine(m.ScopeName);
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    
    }



    public class MyClass
    {
        private int _MyInt;

        public int MyInt
        {
            get { return _MyInt; }
            set { _MyInt = value; }
        }

        public void Write(int i)
        {
            Console.WriteLine(i.ToString());
        }

        public event EventHandler MyEvent;
    }


    public class Animal
    {}
    public class Dog : Animal
    {}
    public class Cat : Animal
    {}
}
