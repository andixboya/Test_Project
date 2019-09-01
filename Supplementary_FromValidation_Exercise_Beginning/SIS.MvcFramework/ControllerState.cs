using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    using Validation;

    public class ControllerState : IControllerState
    {
        public ModelStateDictionary ModelState { get; set; }

        public ControllerState()
        {
            this.Reset();
        }

        public void Reset()
        {
            //1. we create a new dictionary, so we reset eveyrthing.
            this.ModelState = new ModelStateDictionary();
        }

        public void Initialize(Controller controller)
        {
            //2. we take the state of an existing controller
            this.ModelState = controller.ModelState;
        }

        public void SetState(Controller controller)
        {
            //3. we set the state of the target-controller to the state within the static class?
            controller.ModelState = this.ModelState;
        }
    }
}
