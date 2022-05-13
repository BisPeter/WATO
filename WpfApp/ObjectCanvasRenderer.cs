using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Shapes;

    /// <summary>
    /// The game object canvas renderer is an i game object renderer 
    /// and is responsible for the render of game objects as rectangle to the given canvas. 
    /// </summary>
    public class ObjectCanvasRenderer
    {
        /// <summary>
        /// The canvas of the renderer.
        /// </summary>
        private readonly Canvas canvas;


        /// <summary>
        /// The add command, which will be executed, when an game object should be added.
        /// </summary>
        private readonly ICommand addCommand;

        /// <summary>
        /// The remove command, which will be executed, when a rectangle should be removed.
        /// </summary>
        private readonly ICommand removeCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectCanvasRenderer"/> class.
        /// </summary>
        /// <param name="canvas">The canvas, where to render.</param>
        /// <param name="abortCommand">The abort command, which will be executed, when the game is abort.</param>
        /// <param name="addCommand">The add command, which will be executed, when an game object should be added.</param>
        /// <param name="removeCommand">The remove command, which will be executed, when a rectangle should be removed.</param>
        public ObjectCanvasRenderer(Canvas canvas, ICommand addCommand, ICommand removeCommand)
        {
            this.addCommand = addCommand;
            this.removeCommand = removeCommand;
            this.canvas = canvas;
        }


        /// <summary>
        /// This method, take the given data, process it,
        /// and call the add command and remove command, due to what game objects are added or removed,
        /// so what rectangles should be rendered or deleted.
        /// </summary>
        /// <param name="data">The given data.</param>
        /*public void RenderData(GameExchangeData data)
        {
            data.Removed.ForEach(r =>
            {
                this.gameObjects.RemoveAll(go => go.ID == r.ID && go.Origin.Left == r.Origin.Left && go.Origin.Top == r.Origin.Top);
                IEnumerable<IGameObject> removeGOs = this.RectangleAssigns.Keys.Where(go => go.ID == r.ID && go.Origin.Left == r.Origin.Left && go.Origin.Top == r.Origin.Top);
                removeGOs.ToList().ForEach(go =>
                {
                    this.removeCommand.Execute(this.RectangleAssigns[go]);
                    this.RectangleAssigns.Remove(go);
                });
            });

            data.RemovedC.ForEach(r => this.RGBValuesAtPositions.RemoveAll(rgb => rgb.Position.Left == r.Position.Left && rgb.Position.Top == r.Position.Top));

            data.Added.ForEach(g =>
            {
                try
                {
                    if (!this.GameObjects.Exists(go => go.Origin.Left == g.Origin.Left && go.Origin.Top == g.Origin.Top))
                    {
                        g.Accept(this);
                    }
                }
                catch (Exception)
                {
                }
            });
        }*/
    }
}
