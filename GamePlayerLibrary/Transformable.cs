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
        
        public float X, Y, Z;
        public float CenterX, CenterY;

        public float ScaleX = 1;
        public float ScaleY = 1;

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
    }
}
