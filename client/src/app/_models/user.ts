export interface User{
    username: string;
    id: number;
    token: string;
    name: string;
    lastName: string;
    roles: string[];
    selectedRole?: number;
}