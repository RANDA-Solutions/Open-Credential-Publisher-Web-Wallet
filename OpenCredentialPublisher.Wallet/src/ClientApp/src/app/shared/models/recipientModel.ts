import { ApplicationUser } from "./applicationUser";

export class RecipientModel {
  id: number;
  userId: string;
  user: ApplicationUser;
  name: string;
  email: string;
  createdAt: Date;
  modifiedAt: Date;
  isDeleted: boolean;
}
