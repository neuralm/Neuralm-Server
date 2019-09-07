import User from '@/models/user';

/**
 * Represents the IUserService interface.
 */
export default interface IUserService {
    /**
     * Logs out the user.
     */
    logout(): void;

    /**
     * Logs in the user with the given user name and password.
     * @param username The user name.
     * @param password The password.
     * @returns The user object.
     */
    login(username: string, password: string): Promise<User>;

    /**
     * Register the user.
     * @param user The user.
     * @returns A value whether the user was succesfully registered.
     */
    register(user: User): Promise<boolean>;
}
