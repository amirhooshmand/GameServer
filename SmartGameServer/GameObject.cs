namespace SmartGameServer
{
    internal class GameObject
    {
        public GameObject()
        {
        }

        public dynamic Id { get; set; }
        public dynamic Name { get; set; }
        public Rotation Rotation { get; set; }
        public Vector3 Vector { get; set; }
    }
}