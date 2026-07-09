// Filtrar usuarios según las opciones seleccionadas

import { Room } from "../../interfaces/Room/Room";

export const SearchRooms = ({
  rooms,
  searchTerm,
}: {
  rooms: Room[];
  searchTerm: string;
}): Room[] => {
  // Dividir el término de búsqueda en palabras, omitiendo espacios extra
  const searchWords = searchTerm
    .toLowerCase()
    .split(/\s+/)
    .filter((word) => word.trim() !== "");

  // Si no hay término de búsqueda, devolvemos la lista completa
  if (searchWords.length === 0) {
    return rooms;
  }

  return rooms.filter((user) => {
    // Concatenar los campos relevantes en un único string para búsqueda
    const haystack = [
      user.description,
      user.name,
      user.teacherFullName,
      user.location,
      user.capacity,
    ]
      .filter(Boolean)
      .join(" ")
      .toLowerCase();

    // Cada palabra del término de búsqueda debe encontrarse en el haystack
    return searchWords.every((word) => haystack.includes(word));
  });
};
