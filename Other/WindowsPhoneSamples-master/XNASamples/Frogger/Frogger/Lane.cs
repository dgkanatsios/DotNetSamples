/*
 * Videogames Laboratory
 * Project Frogger
 * Developed by Kostas Anagnostou
 * 
 * You are free to use and modify the code in any way for any educational (non-commercial) purpose.
 * 
 * For more game development tutorials (in Greek) visit http://videogameslab.wordpress.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Frogger
{


    class Lane
    {
        Vector2 position;
        TrafficDirection direction;
        float delay;
        List<Car> cars;
        int maxCars;
        Random random;
        int laneHeight;
        Texture2D carTexture;
        Rectangle roadRectagle;
        double elapsedTime;
        int speed;

        int carSize;

        public Lane(Vector2 position, int laneHeight, Rectangle roadRectagle, TrafficDirection direction, float delay, int speed, Texture2D carTexture)
        {
            this.direction = direction;
            this.delay = delay;
            this.laneHeight = laneHeight;
            this.position = position;
            this.roadRectagle = roadRectagle;
            this.carTexture = carTexture;
            this.speed = speed;           

            carSize = 2 * laneHeight;
            random = new Random(DateTime.Now.Millisecond);

            maxCars = roadRectagle.Width / carSize;
            cars = new List<Car>(maxCars);

            for (int i = 0; i < maxCars; i++)
            {
                Rectangle rect;
                if (direction == TrafficDirection.Right)
                {                       
                    rect = new Rectangle(-5 * carSize, (int)position.Y + 2, carSize, laneHeight - 4);
                }
                else
                {
                    rect = new Rectangle(roadRectagle.Width + 5 * carSize, (int)position.Y + 2, carSize, laneHeight - 4);
                }
                //initially, the car will be offscreen. It will be positioned correctly at the first Update.
                cars.Add(new Car(rect, roadRectagle.Width, speed, direction, carTexture));
            }

            elapsedTime = 0.0 ;
        }

        public bool CheckCollision(Rectangle frog)
        {
            foreach (Car car in cars)
            {
                if (car.CheckCollision(frog))
                {
                    return true;
                }
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime -= gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Car car in cars)
            {
                car.Update(gameTime);

                //check if car is outside screen
                if (!car.CheckCollision(roadRectagle))
                {
                    bool collides = false;
                    Rectangle rect;
                    Rectangle collisionRect;

                    //calculate new car Rectangle, based on Lane direction
                    if (direction == TrafficDirection.Right)
                    {
                        rect = new Rectangle(-carSize + 1, (int)position.Y + 2, carSize, laneHeight - 4);
                        collisionRect = rect;
                        //move rectangle to the right laneHeight pixels to make sure two cars don't touch
                        collisionRect.X += laneHeight;
                    }
                    else
                    {
                        rect = new Rectangle(roadRectagle.Width+carSize - 1, (int)position.Y + 2, carSize, laneHeight - 4);
                        collisionRect = rect;
                        //move rectangle to the left laneHeight pixels to make sure two cars don't touch
                        collisionRect.X -= laneHeight;
                    }

                    //check if new car collides with the one in front. If yes, delay adding it to the lane
                    foreach (Car tempCar in cars)
                    {
                        //use the offset rectangle for collision detection
                        if (tempCar.CheckCollision(collisionRect))
                        {
                            collides = true;
                            break;
                        }
                    }

                    //is it time to add a new car?
                    if (elapsedTime <= 0.0 && !collides)
                    {
                        //add a new car with 33.3% chance
                        if (random.Next(3) == 0)
                        {
                            car.Reset(rect, roadRectagle.Width, speed, direction);
                        }
                        elapsedTime = delay;                        
                    }
                }

            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Car car in cars)
            {
                car.Draw(sb);
            }
        }
    }
}
