using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    private async void Awake()
    {
        if(UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            Debug.Log("Services Initializing");
            await UnityServices.InitializeAsync();
        }
        PlayerAccountService.Instance.SignedIn += SignInOrLinkWithUnity;
    }

    void start()
    {
        if(!AuthenticationService.Instance.SessionTokenExists)
        {
            Debug.Log("Session token not found");
            return;
        }
        Debug.Log("Returning player signing in");
        await SignInAnonymouslyAsync();
    }


    public async void StartAnonymousSignIn()
    {
        await SignInAnonymouslyAsync();
    }

    //// Unity Anon sign in
    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
       }
    }
    private async void StartUnitySignInAsync()
{
    if(PlayerAccountService.Instance.IsSignedIn)
    {
        SignInOrLinkWithUnity();
        return;
    }
    try
    {
        await playerAccountService.Instance.StartSignInAsync();
    }
    catch (RequestFailedException ex)
    {
        Debug.LogException(ex);
        SetException(ex);
    }
}

async void SignInOrLinkWithUnity()
{
    try
    {
        await AuthenticationService.Instance.SignInOrLinkWithUnityAsync(PlayerAccountService.Instance.AccessToken);
        m_ExternalIds = GetExternalIds(AuthenticationService.Instance.PlayerInfo);
        UpdateUI();
    }
    catch (RequestFailedException ex)
    {
        Debug.LogException(ex);
        SetException(ex);
    }
}



   private async Task LinkWithUnityAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithUnityAsync(accessToken);
                Debug.Log("Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }


}

//MY ID
//GPEe5wSvdj39LHmthh1cR7k9Jshz  

