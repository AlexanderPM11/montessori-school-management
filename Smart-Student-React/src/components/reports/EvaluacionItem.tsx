import React from "react";

interface Indicator {
  id: number;
  description: string;
}

interface EvaluacionItemProps {
  indicator: Indicator;
  estado: string;
  onEliminar: () => void;
  onChangeEstado: (estado: string) => void;
}

export const EvaluacionItem: React.FC<EvaluacionItemProps> = ({
  indicator,
  estado,
  onEliminar,
  onChangeEstado,
}) => {
  const estados = ["Logrado", "En Proceso", "Iniciando"];

  return (
    <div className="border rounded-lg p-4 bg-gray-50">
      <div className="mb-3 font-normal text-gray-800 text-[14px] md:text-[15px]">
        {indicator.description}
      </div>
      <div className="flex flex-wrap items-center gap-2">
        {estados.map((estadoBtn) => (
          <button
            key={estadoBtn}
            className={`px-2 py-1 md:px-4 md:py-2 text-[14px]  font-semibold  rounded border transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-blue-400 ${
              estado === estadoBtn
                ? "bg-blue-600 text-white border-blue-600"
                : "bg-gray-100 text-gray-800 border-gray-300 hover:bg-gray-200"
            }`}
            onClick={() => onChangeEstado(estadoBtn)}
            type="button"
          >
            {estadoBtn}
          </button>
        ))}

        {/* Eliminar se pone siempre al final y en móvil ocupa todo el ancho */}
        <button
          className="mt-2 sm:mt-0 sm:ml-auto text-red-600 hover:underline focus:outline-none focus:ring-2 focus:ring-red-400"
          onClick={onEliminar}
          type="button"
          aria-label={`Eliminar estado para ${indicator.description}`}
        >
          Eliminar
        </button>
      </div>
    </div>
  );
};
