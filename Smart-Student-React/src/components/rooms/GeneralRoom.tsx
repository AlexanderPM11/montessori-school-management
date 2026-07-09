import { useMediaQuery } from "react-responsive";
import { useEffect } from "react";

import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import { RoomDesplay } from "./RoomDesplay";
import { useRoomsStore } from "../../hooks/store/Room/Rooms.store";
import { DesktopRoomTable } from "./DesktopRoomTable";

export const GeneralRoom = () => {
  const isDesktop = useMediaQuery({ minWidth: 1024 });

  const getAllRoomsAsync = useRoomsStore((state) => state.getAllRoomsAsync);
  const onDelete = useRoomsStore((state) => state.onDelete);

  const isLoading = useRoomsStore((state) => state.loading);
  const currentDisplayRooms = useRoomsStore(
    (state) => state.currentDisplayRooms
  );

  useEffect(() => {
    getAllRoomsAsync();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }
  return isDesktop ? (
    <DesktopRoomTable users={currentDisplayRooms} onDelete={onDelete} />
  ) : (
    <RoomDesplay />
  );
};
