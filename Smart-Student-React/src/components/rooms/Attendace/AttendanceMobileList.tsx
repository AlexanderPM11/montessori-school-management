import React from "react";
import {
  FiX,
  FiCheckCircle,
  FiClock,
  FiXCircle,
  FiAlertCircle,
} from "react-icons/fi";
import { BsFiletypePdf } from "react-icons/bs";
import { ApiResponse } from "../../../interfaces/ApiResponse";
import { AttendanceSummaryViewModel } from "../../../interfaces/Attendace/AttendanceSummaryViewModel";

interface AttendanceMobileListProps {
  attendancesInfo: AttendanceSummaryViewModel;
  selectedDate: string;
  idRoom: number;
  clearStatus: (index: number, id: number) => void;
  handleStatusChange: (
    index: number,
    studentId: number,
    idRoom: number,
    date: string,
    statusType: "isPresent" | "isDelay" | "isAbsent" | "isExcuse"
  ) => void;
  getPdfStudent(
    idStudent: number,
    selectedDate: string,
    onGetPdfStudent: (
      idStudent: number,
      selectedDate: string
    ) => Promise<ApiResponse<string>>,
    attendacesInfo: AttendanceSummaryViewModel
  ): Promise<void>;
  onGetPdfStudent: (
    idStudent: number,
    date: string
  ) => Promise<ApiResponse<string>>;
  menuTriggerRefs: React.MutableRefObject<
    Record<number, HTMLButtonElement | null>
  >;
}

const AttendanceMobileList: React.FC<AttendanceMobileListProps> = ({
  attendancesInfo,
  selectedDate,
  idRoom,
  clearStatus,
  handleStatusChange,
  getPdfStudent,
  onGetPdfStudent,
  menuTriggerRefs,
}) => {
  return (
    <div className="md:hidden">
      {attendancesInfo.attendances.map((student, index) => {
        const studentId = student.idStudent ?? 0;
        return (
          <div key={index} className="border-b border-gray-200 p-4 relative">
            {/* Header */}
            <div className="flex justify-between items-center">
              <div>
                <div className="font-medium text-gray-800">
                  <span className="text-gray-500 mr-2">
                    #{student.numberList}
                  </span>
                  {student.fullNameStudent}
                </div>
              </div>
              <div className="flex gap-2">
                <button
                  onClick={() => clearStatus(index, student.id ?? 0)}
                  className="text-gray-400 hover:text-red-500 transition-colors p-1"
                  title="Limpiar estado"
                >
                  <FiX />
                </button>
                <button
                  ref={(el) => (menuTriggerRefs.current[studentId] = el)}
                  onClick={() =>
                    getPdfStudent(
                      studentId,
                      selectedDate,
                      onGetPdfStudent,
                      attendancesInfo
                    )
                  }
                  className="p-1 text-gray-500 hover:text-gray-700"
                  title="PDF Individual"
                >
                  <BsFiletypePdf className="text-gray-900" />
                </button>
              </div>
            </div>

            {/* Status Buttons */}
            <div className="grid grid-cols-2 gap-2 mt-3">
              {[
                {
                  key: "isPresent",
                  label: "Presente",
                  icon: <FiCheckCircle />,
                  active: student.isPresent,
                  color: "green",
                },
                {
                  key: "isDelay",
                  label: "Tardanza",
                  icon: <FiClock />,
                  active: student.isDelay,
                  color: "yellow",
                },
                {
                  key: "isAbsent",
                  label: "Ausente",
                  icon: <FiXCircle />,
                  active: student.isAbsent,
                  color: "red",
                },
                {
                  key: "isExcuse",
                  label: "Excusa",
                  icon: <FiAlertCircle />,
                  active: student.isExcuse,
                  color: "blue",
                },
              ].map((status) => (
                <button
                  key={status.key}
                  onClick={() =>
                    handleStatusChange(
                      index,
                      studentId,
                      idRoom,
                      selectedDate,
                      status.key as
                        | "isPresent"
                        | "isDelay"
                        | "isAbsent"
                        | "isExcuse"
                    )
                  }
                  className={`flex items-center justify-center gap-1 px-3 py-2 rounded-lg border ${
                    status.active
                      ? `bg-${status.color}-100 border-${status.color}-300 text-${status.color}-800`
                      : "bg-white border-gray-200 text-gray-700 hover:bg-gray-50"
                  }`}
                >
                  {status.icon}
                  {status.label}
                </button>
              ))}
            </div>

            {/* Status Badge */}
            <div className="mt-2">
              {student.isPresent && (
                <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-green-100 text-green-800">
                  <FiCheckCircle className="mr-1" /> Presente
                </span>
              )}
              {student.isDelay && (
                <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-yellow-100 text-yellow-800">
                  <FiClock className="mr-1" /> Tardanza
                </span>
              )}
              {student.isAbsent && (
                <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-red-100 text-red-800">
                  <FiXCircle className="mr-1" /> Ausente
                </span>
              )}
              {student.isExcuse && (
                <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-blue-100 text-blue-800">
                  <FiAlertCircle className="mr-1" /> Excusa
                </span>
              )}
              {!student.isPresent &&
                !student.isDelay &&
                !student.isAbsent &&
                !student.isExcuse && (
                  <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-gray-100 text-gray-800">
                    Sin registrar
                  </span>
                )}
            </div>
          </div>
        );
      })}
    </div>
  );
};

export default AttendanceMobileList;
