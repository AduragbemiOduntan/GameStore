using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GamesEndpoints
    {
        //Naming constants
        const string GetGameEndpointName = "GetGame";

        // Creating an in memory DB for games resource
        private static readonly List<GameSummaryDto> games =
        [
            new (1, "Street Fighter II", "1", 19.99M, new DateOnly(1992, 7, 15)),
            new (2, "Final Fantasy XIV", "4", 59.99M, new DateOnly(2010, 9, 30)),
            new GameSummaryDto(3, "FIFA 23", "2", 66.99M, new DateOnly(2022, 9,27)),
        ];

        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games")
                .WithParameterValidation();
            // GET /games
            group.MapGet("/", (GameStoreContext dbContext) =>
            {
                var games = dbContext.Games.ToList();
                 var gamesDto = new List<GameSummaryDto>();
                foreach(var game in games)
                {
                    var gameDto = game.ToGameSummaryDto();
                    gamesDto.Add(gameDto);
                }
                return gamesDto;
            }); //Example of a minimal API

            // GET /games/1
            group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
            {
                Game? game = dbContext.Games.Find(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

            }).WithName(GetGameEndpointName);

            //POST / games
            group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = newGame.ToEntity();
                game.Genre = dbContext.Genres.Find(newGame.GenreId);

                dbContext.Games.Add(game);   //Or dbContext.Add(game);
                dbContext.SaveChanges();

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameSummaryDto());

            });

            //PUT /games/5
            group.MapPut("/{id}", (int id, UpdateGameDto gameUpdate, GameStoreContext dbContext) =>
            {
                var existingGame = dbContext.Games.Find(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(gameUpdate.ToEntity(id));

                dbContext.SaveChanges();

                return Results.NoContent();
            });

            //DELETE /games/1
            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            });

            return group;

        }
    }
    //Or this, when you are not using "MapGroup" of RouteGroupBuilder class 
    //public static WebApplication MapGamesEndpoints(this WebApplication app)
    //        {
    //      
    //            // GET /games
    //            app.MapGet("games", () => games); //Example of a minimal API

    //            // GET /games/1
    //            app.MapGet("games/{id}", (int id) =>
    //            {
    //                GameSummaryDto? game = games.Find(game => game.Id == id);

    //                return game is null ? Results.NotFound() : Results.Ok(game);
    //            })
    //                .WithName(GetGameEndpointName);

    //            //POST /games
    //            app.MapPost("games", (CreateGameDto newGame) =>
    //            {
    //                GameSummaryDto game = new(
    //                    games.Count + 1,
    //                    newGame.Name,
    //                    newGame.Genre,
    //                    newGame.Price,
    //                    newGame.ReleaseDate);

    //                games.Add(game);

    //                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
    //            });

    //            //PUT /games/5
    //            app.MapPut("games/{id}", (int id, UpdateGameDto gameUpdate) =>
    //            {
    //                var index = games.FindIndex(game => game.Id == id);

    //                if (index == -1)
    //                {
    //                    return Results.NotFound();
    //                }

    //                games[index] = new GameSummaryDto(
    //                    id,
    //                    gameUpdate.Name,
    //                    gameUpdate.Genre,
    //                    gameUpdate.Price,
    //                    gameUpdate.ReleaseDate
    //                    );

    //                return Results.NoContent();
    //            });

    //            //DELETE /games/1
    //            app.MapDelete("games/{id}", (int id) => {
    //                games.RemoveAll(game => game.Id == id);

    //                return Results.NoContent();
    //            });

    //            return app;

    //            // GET /games/id
    //            //app.MapGet("games/{id}", (int id) => games[id]); // ---> This is not a better option to "Find()" because it works with the array positioning and not the resource ID, however it works.

    //            //List<GameSummaryDto> games = new() ----> // Another way to implement the above
    //            //{
    //            //    new (1, "Street Fighter", "Combat", 19.99M, new DateOnly(1992, 7, 15)),
    //            //    new (1, "Street Fighter", "Combat", 19.99M, new DateOnly(1992, 7, 15)),
    //            //};


    //        }
    //    }
}
