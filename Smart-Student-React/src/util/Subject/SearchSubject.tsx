// Filtrar usuarios según las opciones seleccionadas

import { Subject } from "../../interfaces/Subject/Subject";

export const SearchSubject = ({
  subjects,
  searchTerm,
}: {
  subjects: Subject[];
  searchTerm: string;
}): Subject[] => {
  // Dividir el término de búsqueda en palabras, omitiendo espacios extra
  const searchWords = searchTerm
    .toLowerCase()
    .split(/\s+/)
    .filter((word) => word.trim() !== "");

  // Si no hay término de búsqueda, devolvemos la lista completa
  if (searchWords.length === 0) {
    return subjects;
  }

  return subjects.filter((user) => {
    // Concatenar los campos relevantes en un único string para búsqueda
    const haystack = [user.code, user.name, user.description]
      .filter(Boolean)
      .join(" ")
      .toLowerCase();

    // Cada palabra del término de búsqueda debe encontrarse en el haystack
    return searchWords.every((word) => haystack.includes(word));
  });
};
