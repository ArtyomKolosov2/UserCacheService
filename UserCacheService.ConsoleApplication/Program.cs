using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Xml;
using UserCacheService.ConsoleApplication.Dtos;
using UserCacheService.ConsoleApplication.Serializer;

namespace UserCacheService.ConsoleApplication;

public static class Program
{
    private static Url _baseUrl = new("http://localhost:5000");
    
    private const string User = "test";
    
    private const string Password = "test";
    
    private const string CreateUserUrl = "Auth/CreateUser";
    
    private const string RemoveUserUrl = "Auth/RemoveUser";
    
    private const string SetStatusUrl = "Auth/SetStatus";
    
    private const string GetUserInfoUrl = "Public/UserInfo";
    
    public static async Task Main(string[] args)
    {
        ConfigureFlurl();
        Console.WriteLine($"Default url: {_baseUrl}");
        Console.WriteLine("Choose a command from the menu:");

        var isRunning = true;
        while (isRunning)
        {
            OutputMenuOptions();

            var command = Console.ReadLine();
            switch (command)
            {
                case "1":
                    await CreateUser();
                    break;
                case "2":
                    await RemoveUser();
                    break;
                case "3":
                    await SetStatus();
                    break;
                case "4":
                    await GetUser();
                    break;
                case "5":
                    ReplaceBaseUrl();
                    break;
                case "0":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Wrong command input. Try again.");
                    break;
            }
        }
    }

    private static void OutputMenuOptions()
    {
        Console.WriteLine("1 - Create user");
        Console.WriteLine("2 - Remove user");
        Console.WriteLine("3 - Set status");
        Console.WriteLine("4 - Get user info");
        Console.WriteLine("5 - Replace base api URL");
        Console.WriteLine("0 - Exit");
    }

    private static Task RemoveUser() => _baseUrl.AppendPathSegment(RemoveUserUrl).WithBasicAuth(User, Password).PostJsonAsync(new RemoveUserRequestDto
    {
        RemoveUser = new RemoveUserDto
        {
            Id = ParseIntFromConsole("Input user id:"),
        }
    }).PrintResponse();

    private static Task SetStatus() => _baseUrl.AppendPathSegment(SetStatusUrl).WithBasicAuth(User, Password).PostUrlEncodedAsync(new Dictionary<string, string>
    {
        { "id", ParseIntFromConsole("Input user id:").ToString() },
        { "newStatus", GetInputFromConsole("Input new status:") }
    }).PrintResponse();

    private static Task CreateUser() =>
        _baseUrl.AppendPathSegment(CreateUserUrl).WithBasicAuth(User, Password).PostXmlAsync(new CreateUserRequestDto
        {
            User = new UserInfoDto
            {
                Id = ParseIntFromConsole("Input user id:"),
                Name = GetInputFromConsole("Input test name:"),
                Status = GetInputFromConsole("Input status:")
            }
        }).PrintResponse();

    private static Task GetUser() =>
        _baseUrl.AppendPathSegment(GetUserInfoUrl)
            .SetQueryParam("id", ParseIntFromConsole("Input user id:").ToString())
            .GetAsync()
            .PrintResponse();

    private static void ReplaceBaseUrl()
    {
        Console.WriteLine("Enter new base url:");
        var url = Console.ReadLine();
        if (Url.IsValid(url))
        {
            _baseUrl = Url.Parse(url);
            Console.WriteLine($"New url: {_baseUrl}");
        }
        else
            Console.WriteLine("Provider URL is invalid");
    }
    
    private static void ConfigureFlurl()
    {
        FlurlHttp.Configure(settings =>
        {
            settings.JsonSerializer = new SystemJsonSerializer(new JsonSerializerOptions
            {
                IncludeFields = true,
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
            });
        });
    }
    
    private static async Task PrintResponse(this Task<IFlurlResponse> response)
    {
        try
        {
            var flurlResponse = await response;
            await PrintSuccessfulResponse(flurlResponse);
        }
        catch (FlurlHttpTimeoutException)
        {
            throw;
        }
        catch (FlurlParsingException)
        {
            throw;
        }
        catch (FlurlHttpException ex)
        {
            await PrintErrorResponse(ex);
        }
    }

    private static async Task PrintErrorResponse(FlurlHttpException ex)
    {
        Console.WriteLine("Error during request.");
        Console.WriteLine(await ex.GetResponseStringAsync());
    }

    private static async Task PrintSuccessfulResponse(IFlurlResponse response)
    {
        Console.WriteLine($"Response code: {response.StatusCode}");
        Console.WriteLine(await response.GetStringAsync());
    }

    private static int ParseIntFromConsole(string message)
    {
        Console.WriteLine(message);
        int value;
        while (!int.TryParse(Console.ReadLine(), out value))
            Console.WriteLine("Wrong input. Try again.");

        return value;
    }

    private static string GetInputFromConsole(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}