using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using todoApi.Models;
using todoApi.Services;

public class UserService : IService {
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;
    private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();


    public UserService (AppDbContext context, TokenService tokenService) {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<dynamic> UserLogin(LoginDTO user) {
        try
        {
            var userFound = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);
            
            if (userFound == null) {
                throw new InvalidOperationException("User not found!");
            }

            var token = _tokenService.GenerateToken(userFound);

            var result = _passwordHasher.VerifyHashedPassword(user.Email, userFound.Password, user.Password);

            userFound.Password = "";

            if (result == PasswordVerificationResult.Success) {
                return new {
                    user = userFound,
                    token
                };
            }

            throw new UnauthorizedAccessException("Senha incorreta!");
        }
        catch (InvalidOperationException ex)
        {
            // Log ou tratativa adicional, se necessário
            throw new InvalidOperationException($"Erro no login: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Captura de outros erros não esperados
            throw new Exception($"Erro inesperado no login: {ex.Message}");
        }
    } 

    public async Task<IEnumerable<UserModel>> GetUsers() {
        try
        {
            List<UserModel> users = await _context.User.ToListAsync();
            return users;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<UserModel> UserRegister(UserDTO user) {
        try
        {
            string hashedPassword = _passwordHasher.HashPassword(user.Email, user.Password);

            var userModel = new UserModel {
                Name = user.Name,
                Email = user.Email,
                Password = hashedPassword
            };

            var userCreated = await _context.User.AddAsync(userModel);
            await _context.SaveChangesAsync();

            return userCreated.Entity;
        }
        catch (System.Exception)
        {
            throw;
        }
        
    } 
}