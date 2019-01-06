using System;


namespace SmartGameServer
{
    class GameObject
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public Vector3 Vector { set; get; }
        public Rotation Rotation { get; set; }

        public GameObject() { }

        public override string ToString()
        {
            return "{ \"gameObject\": { \"id\": " + Id + ", \"name\": " + Name + ", \"positionX\": " + Vector.x + ", \"positionY\":" + Vector.y + ", \"positionZ\": " + Vector.z + ", \"rotationX\": " + Rotation.x + ", \"rotationY\":" + Rotation.y + ", \"rotationZ\": " + Rotation.z + " } }";
        }
    }
}
