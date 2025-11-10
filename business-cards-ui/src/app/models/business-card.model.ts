export interface BusinessCard {
  id: string;
  name: string;
  gender: string;
  dateOfBirth: string;
  email: string;
  phone: string;
  photo?: string | null;
  address: string;
}
