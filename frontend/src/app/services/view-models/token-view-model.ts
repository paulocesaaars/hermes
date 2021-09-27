import { UserInfoViewModel } from './user-info-view-model';

export interface TokenViewModel {
    accessToken: string;
    user: UserInfoViewModel;
}
