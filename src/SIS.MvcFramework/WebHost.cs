

using SIS.HTTP.Responses;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Routing;

namespace SIS.MvcFramework
{
    using HTTP.Enums;
    using Attributes;
    using MvcFramework.Routing;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {

            //items left for WebHost:

            //1. mapping table
            var serverRoutingTable = new ServerRoutingTable();
            AutoRegisterMappingRoutes(application, serverRoutingTable);

            //2. Config of services (DI)
            application.ConfigureServices();

            //3.Other Configs., for example db, and the routingTable Routes?
            application.Configure(serverRoutingTable);

            //4. Server Initialization (was at the app. before)
            Server server = new Server(8000, serverRoutingTable);
            server.Run();

        }

        public static void AutoRegisterMappingRoutes(IMvcApplication application, IServerRoutingTable serverRoutingTable)
        {
            //we take all of the controllers (first filter is with the name of the controllers)
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                    && typeof(Controller).IsAssignableFrom(type));
            // TODO: RemoveToString from InfoController

            foreach (var controller in controllers)
            {
                //2nd filter is that we want only the actions
                var actions = controller
                    .GetMethods(BindingFlags.DeclaredOnly
                    | BindingFlags.Public
                    | BindingFlags.Instance)
                    .Where(x => !x.IsSpecialName && x.DeclaringType == controller)
                    .Where(m=>m.GetCustomAttributes().All(a=> a.GetType() != typeof(NonActionAttribute)));


                foreach (var action in actions)
                {
                    //3rd we create the path (from the controllerName and actionName)
                    var path = $"/{controller.Name.Replace("Controller", string.Empty)}/{action.Name}";

                    //4th each action will have at least one attribute if it doesn`t we have to make the default settings (get)
                    var attribute = action
                        .GetCustomAttributes()
                        .LastOrDefault(a => 
                            a.GetType().IsSubclassOf(typeof(BaseHttpAttribute))) as BaseHttpAttribute;

                    var httpMethod = HttpRequestMethod.Get;

                    //5th for post/get
                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    //6th  this is for "/"
                    if (attribute?.Url != null)
                    {
                        path = attribute.Url;
                    }

                    //7th {`confirmCreate`} this is for the confirmCase, where we take the name from the attribute and replace it with the one from the method`s name
                    if (attribute?.ActionName != null)
                    {
                        path = $"/{controller.Name.Replace("Controller", string.Empty)}/{attribute.ActionName}";
                    }

                    serverRoutingTable.Add(httpMethod, path, request =>
                    {
                        // request => new UsersController().Login(request)
                        var controllerInstance = (Controller)Activator.CreateInstance(controller);
                        

                        //note:** on each new request, we receive a new controller instance, to which we add the request (and the session storage within the request))
                        controllerInstance.Request = request;

                        //we get the principal , which is like a session object
                        var controllerPrincipal = controllerInstance.User;
                        //Security Authorization -> TODO: Refactor this.


                        //note: here we take the author. attribute from the ACTION , not controller
                        var authorizeAttribute = action.GetCustomAttributes()
                            .LastOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

                        if (authorizeAttribute != null && !authorizeAttribute.IsInAuthority(controllerPrincipal))
                        {
                            //note: ****so for the method/action to go through the validation, he needs both Authorization attribute (default is good) and 
                            //present principal (singed in)

                            // TODO: Redirect to configured URL
                            return new HttpResponse(HttpResponseStatusCode.Forbidden);
                        }

                        //TODO: Redirect to configured URL!


                        //8th because of the conversion below, it knows what the action is i think?
                        var response = action.Invoke(controllerInstance, new object[0]) as ActionResult;
                        return response;

                    });

                    Console.WriteLine(httpMethod + " " + path);
                }
            }

            ;
            // Reflection
            // Assembly
            // typeof(Server).GetMethods()
            // sb.GetType().GetMethods();
            // Activator.CreateInstance(typeof(Server))
            var sb = DateTime.UtcNow;
        }
    }
}
