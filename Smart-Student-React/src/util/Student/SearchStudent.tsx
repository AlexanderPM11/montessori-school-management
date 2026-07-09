// Filtrar usuarios según las opciones seleccionadas

import { Student } from "../../interfaces/Students/Student";

export const SearchStudent = ({
  students,
  searchTerm,
}: {
  students: Student[];
  searchTerm: string;
}): Student[] => {
  // Dividir el término de búsqueda en palabras, omitiendo espacios extra
  const searchWords = searchTerm
    .toLowerCase()
    .split(/\s+/)
    .filter((word) => word.trim() !== "");

  // Si no hay término de búsqueda, devolvemos la lista completa
  if (searchWords.length === 0) {
    return students;
  }

  return students.filter((user) => {
    // Concatenar los campos relevantes en un único string para búsqueda
    const haystack = [
      user.code,
      user.name,
      user.lastname,
      user.direction,
      user.telFather,
      user.telMother,
      user.tel,
      user.estado,
      user.relationPersonLiveWithDesc,
      user.sexDes,
      user.gradeDes,
      user.nacionality,
      user.numberList?.toString(),
    ]
      .filter(Boolean)
      .join(" ")
      .toLowerCase();

    // Cada palabra del término de búsqueda debe encontrarse en el haystack
    return searchWords.every((word) => haystack.includes(word));
  });
};
