using Firebase.Auth;
using System.Threading.Tasks;

namespace SecretMessage.WPF.Stores
{
    public class AuthenticationStore
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider;

        private FirebaseAuthLink? _currentFirebaseAuthLink;
        public User? CurrentUser => _currentFirebaseAuthLink?.User;

        public AuthenticationStore(FirebaseAuthProvider firebaseAuthProvider)
        {
            _firebaseAuthProvider = firebaseAuthProvider;
        }


        public async Task Login(string email, string password)
        {
            _currentFirebaseAuthLink = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
        }
    }
}
