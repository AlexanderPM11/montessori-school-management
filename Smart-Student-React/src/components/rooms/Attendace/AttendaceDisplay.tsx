/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState, useRef } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { FiArrowLeft } from "react-icons/fi";
import { useAttendaceStore } from "../../../hooks/store/Attendace/Attendace.store";

import { handleMarkAllPresent } from "./handleMarkAllPresent";
import { useReportAttendanceStore } from "../../../hooks/store/Attendace/ReportAttendance.store";
import {
  fetchAttendanceData,
  getPdfStudent,
  handleGeneratePdf,
} from "../../../util/Attendance/attendancePdfUtils";
import AttendanceHeader from "./AttendanceHeader";
import AttendanceSummary from "./AttendanceSummary";
import AttendanceTable from "./AttendanceTable";
import AttendanceMobileList from "./AttendanceMobileList";

export const AttendaceDisplay = () => {
  const [searchParams] = useSearchParams();
  const idRoom = Number(searchParams.get("idRoom"));

  const [selectedDate, setSelectedDate] = useState<string>(() => {
    const now = new Date();

    // Convertir a hora "America/Santo_Domingo"
    const formatter = new Intl.DateTimeFormat("en-US", {
      timeZone: "America/Santo_Domingo",
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    });

    const parts = formatter.formatToParts(now);
    const year = parts.find((p) => p.type === "year")?.value;
    const month = parts.find((p) => p.type === "month")?.value;
    const day = parts.find((p) => p.type === "day")?.value;

    const date = new Date(`${year}-${month}-${day}T00:00:00-04:00`);

    // Día de la semana en Santo Domingo
    const dayOfWeek = date.getDay();

    if (dayOfWeek === 6) {
      date.setDate(date.getDate() - 1);
    } else if (dayOfWeek === 0) {
      date.setDate(date.getDate() - 2);
    }

    // Devolver en formato yyyy-MM-dd
    const formatted = date.toISOString().split("T")[0];
    return formatted;
  });

  const menuTriggerRefs = useRef<Record<number, HTMLButtonElement | null>>({});

  const getAttendanceInfo = useAttendaceStore((s) => s.getAttendanceInfo);
  const handleStatusChange = useAttendaceStore((s) => s.handleStatusChange);
  const clearStatus = useAttendaceStore((s) => s.clearStatus);
  const markAllAsPresent = useAttendaceStore((s) => s.markAllAsPresent);

  const getPdf = useReportAttendanceStore((s) => s.getPdf);
  const onGetPdfStudent = useReportAttendanceStore((s) => s.GetPdfStudent);

  const attendacesInfo = useAttendaceStore((s) => s.attendacesInfo);
  const loading = useAttendaceStore((s) => s.loading);

  useEffect(() => {
    fetchAttendanceData(idRoom, selectedDate, getAttendanceInfo);
  }, [selectedDate, idRoom]);

  if (loading) {
    return <div></div>;
  }

  return (
    <div className="p-4 mb-20 md:mb-10 ">
      {/* Header Section */}
      <AttendanceHeader
        selectedDate={selectedDate}
        setSelectedDate={setSelectedDate}
        idRoom={idRoom}
        handleMarkAllPresent={handleMarkAllPresent}
        markAllAsPresent={markAllAsPresent}
        handleGeneratePdf={handleGeneratePdf}
        getPdf={getPdf}
      />

      {/* Summary Cards */}
      <AttendanceSummary
        presentCount={attendacesInfo.presentCount}
        delayCount={attendacesInfo.delayCount}
        absentCount={attendacesInfo.absentCount}
        excuseCount={attendacesInfo.excuseCount}
      />

      <Link
        className="hidden md:flex border border-gray-900 h-10 w-10 rounded-full mb-4
                           items-center justify-center text-[14px] 
                           transition-all duration-200 ease-in-out bg-gray-900 text-white
                           hover:bg-white hover:text-gray-900"
        to={`/rooms/`}
        title="Volver"
      >
        <FiArrowLeft />
      </Link>

      {/* Attendance Table */}
      <div className="bg-white rounded-xl shadow-sm border overflow-hidden">
        {/* Desktop Table */}

        <AttendanceTable
          attendancesInfo={attendacesInfo}
          idRoom={idRoom}
          selectedDate={selectedDate}
          handleStatusChange={handleStatusChange}
          clearStatus={clearStatus}
          getPdfStudent={getPdfStudent}
          menuTriggerRefs={menuTriggerRefs}
          onGetPdfStudent={onGetPdfStudent}
        />

        {/* Mobile List */}
        <AttendanceMobileList
          attendancesInfo={attendacesInfo}
          selectedDate={selectedDate}
          idRoom={idRoom}
          clearStatus={clearStatus}
          handleStatusChange={handleStatusChange}
          getPdfStudent={getPdfStudent}
          onGetPdfStudent={onGetPdfStudent}
          menuTriggerRefs={menuTriggerRefs}
        />
      </div>
    </div>
  );
};
