import {
  FiHash,
  FiUser,
  FiCheckCircle,
  FiClock,
  FiXCircle,
  FiAlertCircle,
  FiX,
} from "react-icons/fi";
import { BsFiletypePdf } from "react-icons/bs";
import { AttendanceSummaryViewModel } from "../../../interfaces/Attendace/AttendanceSummaryViewModel";
import { ApiResponse } from "../../../interfaces/ApiResponse";

interface AttendanceTableProps {
  attendancesInfo: AttendanceSummaryViewModel;
  idRoom: number;
  selectedDate: string;
  handleStatusChange: (
    index: number,
    studentId: number,
    idRoom: number,
    selectedDate: string,
    status: string
  ) => void;
  clearStatus: (index: number, studentId: number) => void;
  getPdfStudent(
    idStudent: number,
    selectedDate: string,
    onGetPdfStudent: (
      idStudent: number,
      selectedDate: string
    ) => Promise<ApiResponse<string>>,
    attendacesInfo: AttendanceSummaryViewModel
  ): Promise<void>;
  menuTriggerRefs: React.MutableRefObject<
    Record<number, HTMLButtonElement | null>
  >;
  onGetPdfStudent: (
    idStudent: number,
    date: string
  ) => Promise<ApiResponse<string>>;
}

const AttendanceTable: React.FC<AttendanceTableProps> = ({
  attendancesInfo,
  idRoom,
  selectedDate,
  handleStatusChange,
  clearStatus,
  getPdfStudent,
  menuTriggerRefs,
  onGetPdfStudent,
}) => {
  return (
    <div className="hidden md:block overflow-x-auto">
      <table className="w-full">
        <thead className="bg-gray-50">
          <tr className="text-left text-gray-600">
            <th className="px-6 py-3 font-medium">
              <FiHash className="inline mr-2" />
              N°
            </th>
            <th className="px-6 py-3 font-medium">
              <FiUser className="inline mr-2" />
              Estudiante
            </th>
            <th className="px-6 py-3 font-medium text-center">Estado</th>
            <th className="px-6 py-3 font-medium text-center">Acción</th>
            <th className="px-6 py-3 font-medium text-center">Opciones</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200">
          {attendancesInfo.attendances.map((student, index) => {
            const studentId = student.idStudent ?? 0;
            return (
              <tr key={index} className="hover:bg-gray-50 transition-colors">
                <td className="px-6 py-4 font-medium text-gray-700">
                  {student.numberList}
                </td>
                <td className="px-6 py-4">
                  <div className="font-medium text-gray-800">
                    {student.fullNameStudent}
                  </div>
                </td>
                <td className="px-6 py-4">
                  <div className="flex justify-center gap-2 flex-wrap">
                    <button
                      onClick={() =>
                        handleStatusChange(
                          index,
                          studentId,
                          idRoom,
                          selectedDate,
                          "isPresent"
                        )
                      }
                      className={`flex items-center gap-1 px-3 py-1 rounded-lg border ${
                        student.isPresent
                          ? "bg-green-100 border-green-300 text-green-800"
                          : "bg-white border-gray-200 text-gray-700 hover:bg-gray-50"
                      }`}
                    >
                      <FiCheckCircle />
                      Presente
                    </button>
                    <button
                      onClick={() =>
                        handleStatusChange(
                          index,
                          studentId,
                          idRoom,
                          selectedDate,
                          "isDelay"
                        )
                      }
                      className={`flex items-center gap-1 px-3 py-1 rounded-lg border ${
                        student.isDelay
                          ? "bg-yellow-100 border-yellow-300 text-yellow-800"
                          : "bg-white border-gray-200 text-gray-700 hover:bg-gray-50"
                      }`}
                    >
                      <FiClock />
                      Tardanza
                    </button>
                    <button
                      onClick={() =>
                        handleStatusChange(
                          index,
                          studentId,
                          idRoom,
                          selectedDate,
                          "isAbsent"
                        )
                      }
                      className={`flex items-center gap-1 px-3 py-1 rounded-lg border ${
                        student.isAbsent
                          ? "bg-red-100 border-red-300 text-red-800"
                          : "bg-white border-gray-200 text-gray-700 hover:bg-gray-50"
                      }`}
                    >
                      <FiXCircle />
                      Ausente
                    </button>
                    <button
                      onClick={() =>
                        handleStatusChange(
                          index,
                          studentId,
                          idRoom,
                          selectedDate,
                          "isExcuse"
                        )
                      }
                      className={`flex items-center gap-1 px-3 py-1 rounded-lg border ${
                        student.isExcuse
                          ? "bg-blue-100 border-blue-300 text-blue-800"
                          : "bg-white border-gray-200 text-gray-700 hover:bg-gray-50"
                      }`}
                    >
                      <FiAlertCircle />
                      Excusa
                    </button>
                  </div>
                </td>
                <td className="px-6 py-4 text-center">
                  <button
                    onClick={() => clearStatus(index, student.id ?? 0)}
                    className="text-gray-500 hover:text-red-500 transition-colors"
                    title="Limpiar estado"
                  >
                    <FiX className="text-lg" />
                  </button>
                </td>
                <td className="px-6 py-4 text-center">
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
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};

export default AttendanceTable;
