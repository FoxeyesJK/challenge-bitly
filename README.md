# challenge-bitly



**Problem**
* Expose an endpoint to provide the average number of daily clicks Bitlinks in the user's default group received from each country


**Tech Stack**
* Language: C#
* Framework: .Net Core 3.1
* Protocol: RESTAPI 
* Deploy: Heroku using docker - https://challenge-bitly.herokuapp.com/countries


**API EndPoints**
* /countries - provide the average number of clicks broken down by country
    * Query Parameters
        * unit: A unit of time
                Default("day")
                Enum("minute", "hour", "day", "week", "month")
        * units: An integer representing the time units to query data for. pass -1  to return all units of time.
                Default(-1)
    * Response
        ```json
        [
            {
                "country": "US",
                "units": 30,
                "unit": "day",
                "total_clicks": 4,      
                "average_clicks": 0.13, 
                "bitlinks": [
                    {
                        "bitlinkId": "bitly.is/3v5Ra7x",
                        "total_clicks": 1,      
                        "average_clicks": 0.03  
                    },
                    {
                        "bitlinkId": "bit.ly/2O8bOmQ",
                        "total_clicks": 3,
                        "average_clicks": 0.1
                    }
                ]
            }
        ]
        ```


**Bitly API Constraints**
* /groups/{group_guid}/bitlinks
    * size param: max 100, default 50 - due to max size of the param, another request is required using pagination.next for bitlinks over size of 100
* /bitlinks/{bitlink}/countries
    * size param: max 15000, default 50 - due to default size of the param, request with max size is required to avoid missing data


**Architecture**

![image](https://user-images.githubusercontent.com/25089799/110382600-22c0b080-8029-11eb-982d-6323cf9f7ab8.png)

**Packages**
* Used only built-in packages 

**Setup Instructions**
* Clone URL: https://github.com/FoxeyesJK/challenge-bitly.git
* Install .Net Core 3.1 SDK - https://dotnet.microsoft.com/download/dotnet/3.1
* Install C# from the editor if needed - No other packages are needed(Used only built-in packages)
* Do not need to install any other packages
* Add Access Token in appsettings.json
* Open terminal
* Run Dotnet App using command "dotnet run"
* Request GET /countries - /coutnries?unit={unit}&units={units}


**Possible Implementations**
* Development Mode: setup for Development & Production
* Unit Testing: along with test cases
* Error Handling: Handling Different Throw Exceptions and returning appropriate status code

**References**
* https://dev.to/alrobilliard/deploying-net-core-to-heroku-1lfe
