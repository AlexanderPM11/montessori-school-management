import { RoomSubject } from "../../interfaces/Room/RoomSubject";

// Filtrar usuarios según las opciones seleccionadas

export const SearchSubjectRoom = ({
  roomSubject,
  searchTerm,
}: {
  roomSubject: RoomSubject[];
  searchTerm: string;
}): RoomSubject[] => {
  // Dividir el término de búsqueda en palabras
  const searchWords = searchTerm
    .toLowerCase()
    .split(" ")
    .filter((word) => word.trim() !== "");

  const filteredroomSubject = roomSubject.filter((subectRoom) => {
    // Verificar si al menos una palabra coincide en cualquier campo
    return searchWords.some(
      (word) =>
        subectRoom.nameGrade.toLowerCase().includes(word) ||
        subectRoom.nameTeacher?.toLowerCase().includes(word)
    );
  });

  return filteredroomSubject;
};
