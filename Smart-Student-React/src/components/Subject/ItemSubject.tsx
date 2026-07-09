import img from "../../assets/images/placeholder.jpg";
import { useEffect, useState } from "react";
import { IoChevronDown, IoChevronUp } from "react-icons/io5";
import { Link } from "react-router-dom";
import { useGeneralStore } from "../../hooks/store/General/General.store";
import { Subject } from "../../interfaces/Subject/Subject";
import { useSubjectStore } from "../../hooks/store/Subject/Subject.store";
import { handleInactiveActive } from "./handleInactiveActive";

interface Props {
  subject: Subject;
  linkToEditOrCreate: string;
}

export const ItemSubject = ({ subject, linkToEditOrCreate }: Props) => {
  const onGetData = useGeneralStore((state) => state.onGetData);

  const [isExpanded, setIsExpanded] = useState(false);
  const onDelete = useSubjectStore((state) => state.onDelete);

  const setIscreating = useGeneralStore((state) => state.setIscreating);

  useEffect(() => {
    onGetData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className="max-w-full md:max-w-[600px]">
      <div className="bg-white border border-gray-300 rounded-lg overflow-hidden">
        {/* Encabezado de la tarjeta */}
        <div
          className="flex items-center justify-between p-3 cursor-pointer"
          onClick={() => setIsExpanded(!isExpanded)}
        >
          <div className="flex items-center gap-2">
            <img
              className="w-10 h-10 rounded-full object-cover"
              src={
                subject.imageUrl && subject.imageUrl !== ""
                  ? subject.imageUrl
                  : img
              }
              alt="User profile"
            />
            <div>
              <p className="text-sm font-bold truncate">
                {subject.name ?? subject.name}
              </p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <span className="w-2 h-2 rounded-full bg-green-500"></span>
            {/* Ícono de flecha */}
            {isExpanded ? (
              <IoChevronUp className="text-gray-600" />
            ) : (
              <IoChevronDown className="text-gray-600" />
            )}
          </div>
        </div>

        {/* Contenido expandible */}
        {isExpanded && (
          <div className="p-3 border-t border-gray-200">
            <div className="flex flex-col gap-2">
              {/* Información básica */}
              <p className="text-xs text-gray-600">
                <strong>Descripción:</strong>{" "}
                {subject.description || "No disponible"}
              </p>
              <p className="text-xs text-gray-600">
                <strong>Código:</strong> {subject.code || "No disponible"}
              </p>
            </div>

            {/* Acciones */}
            <div className="flex gap-2 mt-3">
              <Link
                onClick={(e) => {
                  e.stopPropagation();
                  setIscreating(true);
                }}
                to={`${linkToEditOrCreate}?idSubject=${subject.id}`}
                className="text-sm text-blue-600 hover:text-blue-800"
              >
                Editar
              </Link>
              <button
                className="text-sm text-red-600 hover:text-red-800"
                onClick={(e) => {
                  e.stopPropagation(); // Evita que el acordeón se colapse
                  handleInactiveActive(subject.id, onDelete, subject);
                }}
              >
                Eliminar
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
