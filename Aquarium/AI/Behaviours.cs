using Aquarium.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Aquarium.AI
{
    /// <summary>
    /// The static Behaviours class contains all steering behaviours neccesary for GameObjects.
    /// </summary>
    public static class Behaviours
    {
        /// <summary>
        /// Seeks and moves towards a target vector.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <returns>Vector2 - Steering force</returns>
        public static Vector2 Seek(GameInstance entity, Vector2 target)
        {
            Vector2 desiredVelocity = Vector2.Normalize(target - entity.position) * entity.max_speed;
            return desiredVelocity - entity.velocity;
        }
        
        /// <summary>
        /// Flees away from a target vector.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <returns>Vector2 - Steering force</returns>
        public static Vector2 Flee(GameInstance entity, Vector2 target)
        {
            Vector2 desiredVelocity = Vector2.Normalize(entity.position - target) * entity.max_speed;
            return desiredVelocity - entity.velocity;
        }
        
        /// <summary>
        /// Arrives towards a target vector.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <returns>Vector2 - Steering force</returns>
        public static Vector2 Arrive(GameInstance entity, Vector2 target)
        {
            Vector2 toTarget = target - entity.position;
            float dist = toTarget.Length();

            float rampSpeed = entity.max_speed * dist / 50f;
            float clipSpeed = Math.Min(rampSpeed, entity.max_speed);
            Vector2 desired = (clipSpeed / dist) * toTarget;
            return desired - entity.velocity;
        }

        /// <summary>
        /// Flocks with a list of GameObjects.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="flockWith"></param>
        /// <returns>Vector2 - Steering force</returns>
        public static Vector2 Flock(GameInstance entity, List<GameInstance> flockWith)
        {
            List<GameInstance> neighbors = flockWith.Where(tgt => tgt != entity
            && Vector2.Distance(entity.position, tgt.position) < 50).ToList();

            // prevent execution if list is empty
            if(neighbors.Count == 0) {
                return new Vector2(0, 0);
            }

            // separation
            Vector2 separationForce = new Vector2(0, 0);
            neighbors.ForEach(tgt => {
                Vector2 toAgent = entity.position - tgt.position;
                separationForce += Vector2.Normalize(toAgent / toAgent.Length());
            });
            
            // cohesion
            Vector2 cohesionForce = new Vector2(0, 0);
            Vector2 centerOfMass = new Vector2(0, 0);
            neighbors.ForEach(tgt => {
                centerOfMass += tgt.position;
            });
            centerOfMass /= neighbors.Count;
            cohesionForce = Vector2.Normalize(centerOfMass - entity.position) * entity.max_speed - entity.velocity;

            // alignment
            Vector2 alignmentForce = new Vector2(0, 0);
            Vector2 averageHeading = new Vector2();
            neighbors.ForEach(tgt => {
                averageHeading += tgt.direction;
            });
            alignmentForce = (averageHeading / neighbors.Count) - entity.direction;

            // calculate flocking force
            return (cohesionForce + alignmentForce + separationForce) / 3.0f;
        }
    }
}
