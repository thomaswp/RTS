using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game_Player
{
    public class Transformable : IComparable<Transformable>
    {
        private List<Transformable> children = new List<Transformable>();

        public IEnumerable<Transformable> Children { get { return children.AsReadOnly();  } }

        public Transformable Parent { get; private set; }
        

        public Vector2 Position = new Vector2();
        public Vector2 Scale = new Vector2(1, 1);
        public Vector2 Center = new Vector2();
        public float Z;

        public float ScaleX { get { return Scale.X; } set { Scale.X = value; } }
        public float ScaleY { get { return Scale.Y; } set { Scale.Y = value; } }
        public float CenterX { get { return Center.X; } set { Center.X = value; } }
        public float CenterY { get { return Center.Y; } set { Center.Y = value; } }
        public float X { get { return Position.X; } set { Position.X = value; } }
        public float Y { get { return Position.Y; } set { Position.Y = value; } }

        // 0 to 1
        public float Rotation;

        public Color Color = Colors.White;

        public int Alpha
        {
            get { return Color.Alpha; }
            set { Color = new Color(Color.Red, Color.Green, Color.Blue, value); }
        }

        public virtual void Update()
        {
            children.ForEach(c => c.Update());
            children.Sort();
        }

        public int CompareTo(Transformable other)
        {
            if (other == null) return 1;
            return Z.CompareTo(other.Z);
        }

        public void AddChild(Transformable child)
        {
            if (children.Contains(child)) return;
            if (child.Parent != null) child.Parent.RemoveChild(child);
            child.Parent = this;
            children.Add(child);
        }

        public bool RemoveChild(Transformable child)
        {
            child.Parent = null;
            return children.Remove(child);
        }

        public Matrix GetLocalTransform()
        {
            Matrix matrix = Matrix.CreateTranslation(-CenterX, -CenterY, 0);
            matrix *= Matrix.CreateRotationZ((float)(Rotation * 2 * Math.PI));
            matrix *= Matrix.CreateScale(ScaleX, ScaleY, 1);
            matrix *= Matrix.CreateTranslation(X, Y, 0);
            return matrix;
        }

        public Matrix GetFullTransform()
        {
            Matrix matrix = GetLocalTransform();
            if (Parent != null)
            {
                matrix *= Parent.GetFullTransform();
            }
            return matrix;
        }

        public Vector2 TransformScreenToLocalPoint(Vector2 point)
        {
            return Vector2.Transform(point, Matrix.Invert(GetFullTransform()));
        }

        public Vector2 TransformLocalToScreenPoint(Vector2 point)
        {
            return Vector2.Transform(point, GetFullTransform());
        }
    }
}
