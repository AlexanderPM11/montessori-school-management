import React from "react";
import { FiCheckCircle, FiFileText } from "react-icons/fi";
import { DatePickerWeekdays } from "./DatePickerWeekdays";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { RequestAttendance } from "../../../interfaces/Attendace/RequestAttendance";
import { MarkAllPresentRequest } from "./handleMarkAllPresent";

interface AttendanceHeaderProps {
  selectedDate: string;
  setSelectedDate: (date: string) => void;
  idRoom: number;
  handleMarkAllPresent(
    markAllAsPresent: (request: RequestAttendance) => void,
    { idRoom, dayWeek, idStudent, status }: MarkAllPresentRequest
  ): Promise<void>;
  markAllAsPresent: (request: RequestAttendance) => void;
  handleGeneratePdf(
    idRoom: number,
    selectedDate: string,
    getPdf: (
      idRoom: number,
      selectedDate: string
    ) => Promise<ApiResponse<string>>
  ): Promise<void>;
  getPdf: (idRoom: number, date: string) => Promise<ApiResponse<string>>;
}

const AttendanceHeader: React.FC<AttendanceHeaderProps> = ({
  selectedDate,
  setSelectedDate,
  idRoom,
  handleMarkAllPresent,
  markAllAsPresent,
  handleGeneratePdf,
  getPdf,
}) => {
  return (
    <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-6 gap-4">
      <h1 className="text-2xl font-bold text-gray-800">
        Registro de Asistencia
      </h1>
      <div className="flex flex-col md:flex-row gap-3">
        <div className="relative">
          <div className="relative max-w-[180px]">
            <DatePickerWeekdays
              selectedDate={selectedDate}
              onChange={(value) => setSelectedDate(value)}
              label=""
              className="max-w-[200px]"
            />
          </div>
        </div>
        <button
          onClick={() => {
            const request = {
              idRoom: idRoom,
              dayWeek: selectedDate,
              idStudent: 0,
              status: "",
            };
            handleMarkAllPresent(markAllAsPresent, request);
          }}
          className="bg-indigo-600 hover:bg-indigo-700 text-white font-medium px-4 py-2 rounded-lg shadow-sm flex items-center gap-2 transition-colors"
        >
          <FiCheckCircle className="text-lg" />
          <span>Marcar Todos como Presentes</span>
        </button>
        <button
          onClick={async () => {
            await handleGeneratePdf(idRoom, selectedDate, getPdf);
          }}
          className="bg-transparent border border-emerald-600 text-emerald-600 hover:bg-emerald-600 hover:text-white font-medium px-4 py-2 rounded-lg shadow-sm flex items-center gap-2 transition-colors"
        >
          <FiFileText className="text-lg" />
          <span>Generar PDF</span>
        </button>
      </div>
    </div>
  );
};

export default AttendanceHeader;
