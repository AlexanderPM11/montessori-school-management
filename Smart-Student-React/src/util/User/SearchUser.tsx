import { User } from "../../interfaces/Auth/User";

// Filtrar usuarios según las opciones seleccionadas

export const searchUser = ({
  users,
  searchTerm,
}: {
  users: User[];
  searchTerm: string;
}): User[] => {
  // Dividir el término de búsqueda en palabras
  const searchWords = searchTerm
    .toLowerCase()
    .split(" ")
    .filter((word) => word.trim() !== "");

  const filteredUsers = users.filter((user) => {
    // Verificar si al menos una palabra coincide en cualquier campo
    return searchWords.some(
      (word) =>
        user.userName.toLowerCase().includes(word) ||
        user.lastName?.toLowerCase().includes(word) ||
        user.email.toLowerCase().includes(word)
    );
  });

  return filteredUsers;
};
