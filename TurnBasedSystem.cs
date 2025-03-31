using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;


namespace MonoGame
{
    public class TurnBasedSystem : Component, IUpdatable
    {
        public List<Actor> Actors;
        private int order = 0;
        public TurnBasedSystem()
        {
            Actors = new List<Actor>();
        }

        public void AddActor(Actor actor)
        {
            Actors.Add(actor);
        }

        public void Update()
        {
            UpdateTurn();
        }
        public void UI()
        {
            foreach (var actor in Actors)
            {

            }
        }

        public void RemoveActor(Actor actor)
        {
            Actors.Remove(actor);
            actor.Destroy();
        }

        public void UpdateTurn()
        {
            //Debug.Log(Actors.Count);
            //Debug.Log("Update turn started");
            if (order < Actors.Count)
            {
                //Debug.Log("order < Actors.Count started");
                Actor actorTurn = Actors[order];
                //Debug.Log(actorTurn.Name);

                // Start turn if it's this actor's turn
                if (actorTurn.isTurn)
                {
                    //Debug.Log("is turn started");
                    actorTurn.StartTurn();
                }
                else
                {
                    //Debug.Log("wasnt there turn");
                    order++;
                    actorTurn.StartTurn();
                    //Debug.Log(actorTurn.Name);
                    //Debug.Log(order);
                }
                //Debug.Log("Hit the bottom");
            }
            else
            {
                //Debug.Log("hit the max actor order");
                order = 0;// Reset the order after all actors have had a turn
            }

        }
    }
}
