import React, { useState, useEffect, useRef } from "react";

interface Indicator {
  id: number;
  description: string;
}

interface EvaluacionItemProps {
  indicator: Indicator;
  calification: number | null;
  onEliminar: () => void;
  onChangeEstado: (estado: string) => void;
}

export const EvaluacionItem: React.FC<EvaluacionItemProps> = ({
  indicator,
  calification,
  onEliminar,
  onChangeEstado,
}) => {
  const [localCalification, setLocalCalification] = useState<string>(() =>
    calification !== null && calification !== undefined
      ? String(calification)
      : ""
  );

  const prevCalification = useRef<string | null>(null);
  const isDeleting = useRef(false); // Flag para saber si estamos eliminando

  useEffect(() => {
    const current =
      calification !== null && calification !== undefined
        ? String(calification)
        : "";
    setLocalCalification(current);
    prevCalification.current = current;
  }, [calification]);

  useEffect(() => {
    if (prevCalification.current === localCalification) return;

    const timeout = setTimeout(() => {
      if (localCalification === "") {
        // Solo llamar onChangeEstado si no estamos eliminando
        if (!isDeleting.current) {
          onChangeEstado("");
        }
      } else {
        const num = parseInt(localCalification, 10);
        if (!isNaN(num)) {
          const bounded = Math.min(Math.max(num, 0), 100);
          onChangeEstado(String(bounded));
        }
      }
      prevCalification.current = localCalification;
      isDeleting.current = false; // Reseteamos el flag
    }, 2000);

    return () => clearTimeout(timeout);
  }, [localCalification, onChangeEstado]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;

    if (value === "") {
      setLocalCalification("");
      return;
    }

    const num = parseInt(value, 10);
    if (!isNaN(num) && num <= 100) {
      setLocalCalification(value);
    }
  };

  return (
    <div className="border rounded-lg p-4 bg-gray-50">
      <div
        className="mb-3 font-normal text-gray-800 text-[14px] md:text-[15px]"
        dangerouslySetInnerHTML={{ __html: indicator.description }}
      />

      <div className="flex items-center gap-4">
        <label className="text-gray-700 text-sm">
          Calificación:
          <input
            type="text"
            inputMode="numeric"
            pattern="\d*"
            value={localCalification}
            onChange={handleChange}
            className="ml-2 w-20 px-2 py-1 border rounded focus:outline-none focus:ring focus:ring-blue-300"
            placeholder="0 - 100"
          />
        </label>

        <button
          className="text-red-600 hover:underline focus:outline-none focus:ring-2 focus:ring-red-400 ml-auto"
          onClick={() => {
            isDeleting.current = true;
            setLocalCalification("");
            onEliminar();
          }}
          type="button"
        >
          Eliminar
        </button>
      </div>
    </div>
  );
};
