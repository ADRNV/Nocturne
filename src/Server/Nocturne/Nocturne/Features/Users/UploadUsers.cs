using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Core.Models;
using Nocturne.Infrastructure.Security.Entities;
using System.ComponentModel;
using System.Globalization;
using System.Security.Claims;

namespace Nocturne.Features.Users
{
    public class UploadUsers
    {
        public record Command(IFormFile File) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly UserManager<EntityUser> _userManager;

            private readonly IPasswordHasher<EntityUser> _passwordHasher;

            private readonly IMapper _mapper;

            public Handler(UserManager<EntityUser> userManager,IUserStore<EntityUser> userStore, IPasswordHasher<EntityUser> passwordHasher, IMapper mapper)
            {
                _userManager = userManager;

                _passwordHasher = passwordHasher;
  
                _mapper = mapper;
            }

            //TODO: Have sense create queue, buffer table records shoud be created by job 
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                using MemoryStream fileBytes = new MemoryStream();

                List<bool> createdRecords = new List<bool>();

                using (var reader = request.File.OpenReadStream())
                {
                    using var streamReader = new StreamReader(reader);

                    using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        await foreach(var row in csvReader.GetRecordsAsync<FileUser>())
                        {
                            var user = _mapper.Map<EntityUser>(row);

                            user.PasswordHash = _passwordHasher.HashPassword(user, row.Pasword);

                            var createuser = await _userManager.CreateAsync(user);

                            var claims = new Claim[]
                            {
                                new Claim(ClaimTypes.Role, row.Role),
                                new Claim(ClaimTypes.Name, row.UserName),
                                new Claim(ClaimTypes.Email, row.Login)
                            };

                            await _userManager.AddToRoleAsync(user, row.Role);
                            await _userManager.AddClaimsAsync(user, claims);

                            if (createuser.Errors.Count() != 0)
                            {
                                return false;
                            }
                        }
                    }

                }

                return true;
            }
        }
    }
}
