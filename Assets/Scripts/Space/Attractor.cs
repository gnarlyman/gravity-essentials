using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Space
{
    public class Attractor : MonoBehaviour
    {
        private const float G = 6.67408f;

        private static List<Attractor> _attractors;

        public Rigidbody rb;
        public Attractor orbit;
        public float orbitAngleDegrees;
        public Vector3 orbitAngleAxis;
        public float orbitPeriod;
        public bool initialized;
        public Attractor closest;
        public float closestDistance;

        private bool _hasOrbit;

        private void FixedUpdate()
        {
            foreach (var attractor in _attractors.Where(attractor => attractor != this))
            {
                Attract(attractor);
            }

            if (!_hasOrbit) return;
            
            var direction = rb.position - orbit.transform.position;
            var distance = direction.magnitude;
            
            if (!initialized && orbit.initialized)
            {
                var velocity = CalculateOrbitVelocity(orbit, distance);
                orbitPeriod = CalculateOrbitalPeriod(orbit, distance);
            
                var rotated = Quaternion.AngleAxis(orbitAngleDegrees, orbitAngleAxis) * direction;
                var force = rotated.normalized * velocity;

                rb.velocity = orbit.rb.velocity + force;
                initialized = true;
            }
            else if (initialized)
            {
                orbitPeriod = CalculateOrbitalPeriod(orbit, distance);    
            }
        }

        private void OnEnable()
        {
            if (_attractors == null)
            {
                _attractors = new List<Attractor>();
            }
            
            _attractors.Add(this);

        }

        private void Start()
        {
            if (orbit == null)
            {
                initialized = true;
                return;
            }
            _hasOrbit = true;
        }

        private void Update()
        {

        }

        private void OnDisable()
        {
            _attractors.Remove(this);
        }

        private void Attract(Attractor objToAttract)
        {
            var rbToAttract = objToAttract.rb;

            var direction = rb.position - rbToAttract.position;
            var distance = direction.magnitude;

            if (closestDistance < 1 || distance < closestDistance)
            {
                closestDistance = distance;
                closest = objToAttract;
            }

            if (distance < 1f)
                return;

            var forceMagnitude = G * (rb.mass * rbToAttract.mass) / 
                                 Mathf.Pow(distance, 2);

            var force = direction.normalized * forceMagnitude;
            
            rbToAttract.AddForce(force);
        }

        private static float CalculateOrbitVelocity(Attractor objectToOrbit, float distance)
        {
            return Mathf.Sqrt(G * objectToOrbit.rb.mass / distance);
        }

        private static float CalculateOrbitalPeriod(Attractor objectToOrbit, float distance)
        {
            // Orbital Period
            // T = SQRT [(4 • pi^2 • R^3) / (G*Mcentral)]
            var t = Mathf.Sqrt((4 * Mathf.Pow(Mathf.PI, 2) * 
                                Mathf.Pow(distance, 3)) / 
                               (G * objectToOrbit.rb.mass));
            return t;
        }
        
    }
}