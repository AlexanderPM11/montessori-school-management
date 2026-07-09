import { ReactNode } from "react";

interface DialogProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  titleColor?: string; // nuevo prop opcional
  children: ReactNode;
}

export const DialogCusmtom = ({
  isOpen,
  onClose,
  title,
  titleColor = "text-gray-800", // color por defecto
  children,
}: DialogProps) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-white rounded-lg shadow-lg w-11/12 max-w-md p-6 relative">
        {/* Botón cerrar */}
        <button
          className="absolute top-3 right-3 text-gray-500 hover:text-gray-700"
          onClick={onClose}
        >
          ✕
        </button>

        {/* Título */}
        {title && (
          <h2 className={`text-lg font-bold mb-4 ${titleColor}`}>{title}</h2>
        )}

        {/* Contenido */}
        <div>{children}</div>
      </div>
    </div>
  );
};
