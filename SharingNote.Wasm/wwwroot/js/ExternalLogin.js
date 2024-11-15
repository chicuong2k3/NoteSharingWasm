let signInInProgress = false; // Flag to check if sign-in is in progress

async function googleSignIn(clientId) {
    return new Promise((resolve, reject) => {
        if (signInInProgress) {
            console.warn("Google Sign-In is already in progress.");
            reject("Sign-in already in progress.");
            return;
        }

        try {
            if (typeof google === 'undefined' || !google.accounts) {
                console.error("Google Sign-In API not loaded.");
                reject("Google Sign-In API not loaded.");
                return;
            }

            signInInProgress = true; // Set the sign-in flag to true

            // Initialize the Google Sign-In API with FedCM enabled
            google.accounts.id.initialize({
                client_id: clientId,
                callback: (response) => {
                    signInInProgress = false; // Reset flag on completion
                    if (response.credential) {
                        resolve(response.credential); // Resolve with the ID token on success
                    } else {
                        reject("No credential received.");
                    }
                },
                ux_mode: 'popup', // 'popup' mode recommended for better user experience
                auto_select: false, // Avoids auto-select issues when no session is active
                'data-fedcm': true // Enable FedCM opt-in for testing purposes
            });

            // Display the prompt and handle the different prompt moments
            google.accounts.id.prompt((notification) => {
                if (notification.isNotDisplayed()) {
                    const reason = notification.getNotDisplayedReason();
                    console.warn("Google Sign-In prompt not displayed. Reason:", reason);

                    // Specific handling for 'opt_out_or_no_session' and other reasons
                    if (reason === "opt_out_or_no_session") {
                        signInInProgress = false; // Reset flag if the prompt fails or is dismissed
                        reject("Prompt not displayed due to opt-out or lack of session. Please ensure you are logged in.");
                    } else {
                        signInInProgress = false; // Reset flag if the prompt fails or is dismissed
                        reject(`Prompt not displayed: ${reason}`);
                    }
                }

                if (notification.isSkippedMoment()) {
                    console.warn("Google Sign-In prompt skipped. Reason:", notification.getSkippedReason());
                    signInInProgress = false; // Reset flag if the prompt fails or is dismissed
                    reject(`Prompt skipped: ${notification.getSkippedReason()}`);
                }

                if (notification.isDismissedMoment()) {
                    console.warn("Google Sign-In prompt dismissed by the user.");
                    signInInProgress = false; // Reset flag if the prompt fails or is dismissed
                    reject("Prompt dismissed by the user.");
                }
            });
        } catch (error) {
            signInInProgress = false; // Ensure flag is reset on error
            console.error("Error during Google Sign-In initialization:", error);
            reject(error);
        }
    });
}
