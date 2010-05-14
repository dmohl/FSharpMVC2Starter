namespace FSharpMVC2.Web.Models
open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel
open System.Web.Security
open System.Globalization

[<AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)>]
[<Sealed>] 
type PropertiesMustMatchAttribute = 
    inherit ValidationAttribute 
        val typeId : obj
        val mutable originalProperty : string
        val mutable confirmProperty : string
        new (originalProperty, confirmProperty) = 
            {inherit ValidationAttribute("'{0}' and '{1}' do not match."); originalProperty = originalProperty; 
             confirmProperty = confirmProperty; typeId = new Object()} 
        member x.OriginalProperty with get() = x.originalProperty and set(value) = x.originalProperty <- value
        member x.ConfirmProperty with get() = x.confirmProperty and set(value) = x.confirmProperty <- value
        override x.TypeId with get() = x.typeId
        override x.FormatErrorMessage name =
            String.Format(CultureInfo.CurrentUICulture, x.ErrorMessageString, x.OriginalProperty, x.ConfirmProperty)
        override x.IsValid value =
            let properties = TypeDescriptor.GetProperties value
            let originalValue = properties.Find(x.OriginalProperty, true).GetValue(value)
            let confirmValue = properties.Find(x.ConfirmProperty, true).GetValue(value)
            Object.Equals(originalValue, confirmValue)

[<AttributeUsage(AttributeTargets.Field ||| AttributeTargets.Property, AllowMultiple = false, Inherited = true)>]
[<Sealed>] 
type ValidatePasswordLengthAttribute = 
    val minCharacters : int
    new () = {inherit ValidationAttribute("'{0}' must be at least {1} characters long."); 
                minCharacters = Membership.Provider.MinRequiredPasswordLength} 
    inherit ValidationAttribute 
        override x.FormatErrorMessage name =
            String.Format(CultureInfo.CurrentUICulture, x.ErrorMessageString, name, x.minCharacters)
        override x.IsValid value =
            let valueAsString = value :?> string
            (valueAsString <> null && valueAsString.Length >= x.minCharacters)

[<PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")>]
type ChangePasswordModel = 
    val mutable oldPassword : string
    val mutable newPassword : string
    val mutable confirmPassword : string

    [<Required>]
    [<DataType(DataType.Password)>]
    [<DisplayName("Current password")>]
    member x.NewPassword with get() = x.oldPassword and set(value) = x.oldPassword <- value
    
    [<Required>]
    [<DataType(DataType.Password)>]
    [<ValidatePasswordLength>]
    [<DisplayName("New password")>]
    member x.OldPassword with get() = x.newPassword and set(value) = x.newPassword <- value

    [<Required>]
    [<DataType(DataType.Password)>]
    [<DisplayName("Confirm new password")>]
    member x.ConfirmPassword with get() = x.confirmPassword and set(value) = x.confirmPassword <- value

type LogOnModel = 
    val mutable userName : string
    val mutable password : string
    val mutable rememberMe : bool

    [<Required>]
    [<DisplayName("User name")>]
    member x.UserName with get() = x.userName and set(value) = x.userName <- value

    [<Required>]
    [<DataType(DataType.Password)>]
    [<DisplayName("Password")>]
    member x.Password with get() = x.password and set(value) = x.password <- value

    [<DisplayName("Remember me?")>]
    member x.RememberMe with get() = x.rememberMe and set(value) = x.rememberMe <- value

[<PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")>]
type RegisterModel = 
    val mutable userName : string
    val mutable email : string
    val mutable password : string
    val mutable confirmPassword : string

    [<Required>]
    [<DisplayName("User name")>]
    member x.UserName with get() = x.userName and set(value) = x.userName <- value

    [<Required>]
    [<DataType(DataType.EmailAddress)>]
    [<DisplayName("Email address")>]
    member x.Email with get() = x.email and set(value) = x.email <- value

    [<Required>]
    [<DataType(DataType.Password)>]
    [<DisplayName("Password")>]
    [<ValidatePasswordLength>]
    member x.Password with get() = x.password and set(value) = x.password <- value

    [<Required>]
    [<DataType(DataType.Password)>]
    [<DisplayName("Confirm password")>]
    member x.ConfirmPassword with get() = x.confirmPassword and set(value) = x.confirmPassword <- value

type IMembershipService = interface
    abstract MinPasswordLength : int with get
    abstract ValidateUser : string*string -> bool
    abstract CreateUser : string*string*string -> MembershipCreateStatus
    abstract ChangePassword : string*string*string -> bool
end    

