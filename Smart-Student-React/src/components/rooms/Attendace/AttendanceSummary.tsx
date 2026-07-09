import React from "react";
import {
  FiCheckCircle,
  FiClock,
  FiXCircle,
  FiAlertCircle,
} from "react-icons/fi";

interface AttendanceSummaryProps {
  presentCount: number;
  delayCount: number;
  absentCount: number;
  excuseCount: number;
}

const AttendanceSummary: React.FC<AttendanceSummaryProps> = ({
  presentCount,
  delayCount,
  absentCount,
  excuseCount,
}) => {
  return (
    <div className="grid grid-cols-2 md:grid-cols-4 gap-3 mb-6">
      <div className="bg-white p-3 rounded-lg shadow-sm border border-green-100">
        <div className="flex items-center gap-2">
          <FiCheckCircle className="text-green-500 text-xl" />
          <span className="text-gray-600">Presentes</span>
        </div>
        <div className="text-2xl font-bold text-green-600 mt-1">
          {presentCount}
        </div>
      </div>

      <div className="bg-white p-3 rounded-lg shadow-sm border border-yellow-100">
        <div className="flex items-center gap-2">
          <FiClock className="text-yellow-500 text-xl" />
          <span className="text-gray-600">Tardanzas</span>
        </div>
        <div className="text-2xl font-bold text-yellow-600 mt-1">
          {delayCount}
        </div>
      </div>

      <div className="bg-white p-3 rounded-lg shadow-sm border border-red-100">
        <div className="flex items-center gap-2">
          <FiXCircle className="text-red-500 text-xl" />
          <span className="text-gray-600">Ausentes</span>
        </div>
        <div className="text-2xl font-bold text-red-600 mt-1">
          {absentCount}
        </div>
      </div>

      <div className="bg-white p-3 rounded-lg shadow-sm border border-blue-100">
        <div className="flex items-center gap-2">
          <FiAlertCircle className="text-blue-500 text-xl" />
          <span className="text-gray-600">Excusas</span>
        </div>
        <div className="text-2xl font-bold text-blue-600 mt-1">
          {excuseCount}
        </div>
      </div>
    </div>
  );
};

export default AttendanceSummary;
