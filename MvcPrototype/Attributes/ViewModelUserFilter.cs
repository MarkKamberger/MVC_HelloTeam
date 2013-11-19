using System.Web.Mvc;
using MvcPrototype.BaseModels;


namespace MvcPrototype
{
    /// <summary>
    /// Add this attribute to an action in order to automatically inject the
    /// current user's information into the model (which must be derived from a
    /// BaseViewModel). Prevents the need to pass the currently logged in user
    /// info from the controller into every single view model.
    /// </summary>
    public class ViewModelUserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            BaseModel model;
            var controller = filterContext.Controller as BaseController;

            if (filterContext.Controller.ViewData.Model == null)
            {
                model = new BaseModel();
                filterContext.Controller.ViewData.Model = model;
            }
            else
            {
                model = filterContext.Controller.ViewData.Model as BaseModel;
            }

            // Setup data.
            if (model != null && controller.CurrentUser != null)
            {
                //model.CurrentRole = controller.CurrentUser.Role;
                //model.CurrentUserId = controller.CurrentUser.Id;
                
              
            }

            base.OnActionExecuted(filterContext);
        }
    }
}