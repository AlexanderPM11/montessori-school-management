/* eslint-disable react-hooks/exhaustive-deps */
import { useState, useEffect } from "react";
import { ToastifyCustom } from "../../../util/ToastifyCustom ";

interface DatePickerWeekdaysProps {
  selectedDate: string;
  onChange: (date: string) => void;
  minDate?: string;
  maxDate?: string;
  className?: string;
  label?: string;
}

export const DatePickerWeekdays = ({
  selectedDate,
  onChange,
  minDate,
  maxDate,
  className = "",
  label = "Seleccionar fecha (L-V)",
}: DatePickerWeekdaysProps) => {
  const [internalDate, setInternalDate] = useState(selectedDate);

  // Verificar si es día de semana (lunes a viernes)
  const isWeekday = (dateString: string) => {
    const date = new Date(dateString);
    const day = date.getDay(); // 0 domingo, 6 sábado
    return day !== 6 && day < 5; // Lunes a viernes
  };

  // Manejar cambio de fecha
  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newDate = e.target.value;
    if (isWeekday(newDate)) {
      setInternalDate(newDate);
      onChange(newDate);
    } else {
      ToastifyCustom({
        message: "Seleccione un día de Lunes a Viernes",
        type: "error",
      });
      e.target.value = internalDate;
    }
  };

  // Sincronizar con el valor externo
  useEffect(() => {
    if (selectedDate !== internalDate) {
      if (selectedDate && isWeekday(selectedDate)) {
        setInternalDate(selectedDate);
      } else if (selectedDate) {
        // Aquí podrías forzar una fecha válida si quieres
        // Por ahora sólo se ignora fecha inválida del prop externo
      }
    }
  }, [selectedDate]);

  return (
    <div className={`relative ${className}`}>
      {label && (
        <label
          htmlFor="weekday-date-picker"
          className="block text-gray-700 mb-1 font-medium"
        >
          {label}
        </label>
      )}
      <input
        id="weekday-date-picker"
        type="date"
        value={internalDate}
        min={minDate}
        max={maxDate}
        onChange={handleDateChange}
        className="appearance-none bg-white border border-gray-300 rounded-lg px-3 py-2 text-gray-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 w-full"
      />
    </div>
  );
};
