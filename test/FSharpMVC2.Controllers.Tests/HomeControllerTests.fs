module HomeControllerTests

open System
open NUnit.Framework
open SpecUnit
open FSharpMVC2.Web.Controllers
open System.Web
open System.Web.Mvc

[<TestFixture>]      
type HomeController__when_calling_the_index_action () =   
    [<DefaultValue(false)>]  
    val mutable result : string
    inherit SpecUnit.ContextSpecification()
        override x.Because () =
            x.result <- ""
        [<Test>]    
        member x.should_have_the_expected_message_in_view_data () =    
            x.result.ShouldEqual("Welcome to ASP.NET MVC!") |> ignore
