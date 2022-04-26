export interface AuthResponse {
    isAuthSuccessful: boolean;
    token: string;
    is2StepVerificationRequired: boolean;
    provider: string;
}