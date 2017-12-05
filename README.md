# Ruling
> A validation library that is simple and small

## Example

Let's say we have the following `User` model:
```cs
class User
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

Both `Username` and `Password` are required fields.
To validate our model we can use the following code.
```cs
// create a new rule
var usernameRequired = Required<User>(f => f.Username);

// create Ruling, which contains a set of rules
var ruling = CreateRuling(
    usernameRequired,
    Required<User>(f => f.Password, message: "A custom message") // create an inline rule with a custom message
);

// now let's validate our model!
var result = ruling(new User());

// we can check if our model is valid
var valid = result.Valid;

// we can get our error messages
var errors = result.Errors;
```

## Contributing
*Read our [contributing guide](CONTRIBUTING.md) if you're looking to contribute.*

## LICENSE
MIT