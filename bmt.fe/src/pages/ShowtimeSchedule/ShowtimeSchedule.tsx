import React, { useEffect, useState } from "react";
import { Collapse, Tag, Button } from "antd";
import { useSelector } from "react-redux";
import ShowTimeService from "../../services/ShowTimeService";
import "./ShowtimeSchedule.css";
import Seat from "../Seat/Seat";
import { ShowTime } from "../../models/ShowTimeSearchModelRes";
import { SearchShowTimeModelReq } from "../../models/SearchShowTimeModelReq";
import FoodCombo from "../FoodCombo/FoodCombo";

const { Panel } = Collapse;

interface GroupedShowtimes {
  [roomId: number]: {
    roomName: string;
    showtimes: string[];
  };
}

interface ShowtimeScheduleProps {
  movieId: number;
  onSelectSeat: (selectedSeats: string[]) => void;
}

const ShowtimeSchedule: React.FC<ShowtimeScheduleProps> = ({
  movieId,
  onSelectSeat,
}) => {
  const darkMode = useSelector((state: any) => state.theme.darkMode);
  const [showtimes, setShowtimes] = useState<ShowTime[]>([]);
  const [listRoom, setListRoom] = useState<ShowTime[]>([]);
  const [groupedShowtimes, setGroupedShowtimes] = useState<GroupedShowtimes>(
    {}
  );
  const [selectedShowtimeDate, setSelectedShowtimeDate] = useState<Date>();
  const [selectedRoomId, setSelectedRoomId] = useState<number | null>(null);

  const listSeats = (listSeat: string[]) => {
    onSelectSeat(listSeat);
  };

  useEffect(() => {
    if (showtimes.length > 0) {
      const sortedShowtimes = [...showtimes].sort(
        (a, b) =>
          new Date(a.startTime).getTime() - new Date(b.startTime).getTime()
      );
      setSelectedShowtimeDate(sortedShowtimes[0].startTime);
    }
  }, [showtimes]);

  useEffect(() => {
    const fetchShowTime = async () => {
      try {
        const now = new Date();
        const params: SearchShowTimeModelReq = {
          keySearch: "",
          pageSize: 10,
          pageIndex: 1,
          fromDate: new Date(),
          toDate: new Date(now.getFullYear(), now.getMonth() + 1, 0),
          movieId: movieId,
          roomId: undefined,
        };
        const showtime: ShowTime[] | undefined =
          await ShowTimeService.SearchShowTime(params);
        setShowtimes(Array.isArray(showtime) ? showtime : []);
      } catch (error) {
        console.log("Load showtimes failed", error);
      }
    };
    fetchShowTime();
  }, [movieId]);

  useEffect(() => {
    const fetchShowTime = async () => {
      try {
        if (!selectedShowtimeDate) return;
        const params: SearchShowTimeModelReq = {
          keySearch: "",
          pageSize: 10,
          pageIndex: 1,
          fromDate: selectedShowtimeDate,
          toDate: selectedShowtimeDate,
          movieId: movieId,
          roomId: undefined,
        };
        const showtime: ShowTime[] | undefined =
          await ShowTimeService.SearchShowTime(params);
        setListRoom(Array.isArray(showtime) ? showtime : []);
      } catch (error) {
        console.log("Load showtimes failed", error);
      }
    };
    fetchShowTime();
  }, [selectedShowtimeDate]);

  useEffect(() => {
    const grouped = listRoom.reduce<GroupedShowtimes>((acc, showtime) => {
      if (!acc[showtime.roomId]) {
        acc[showtime.roomId] = {
          roomName: showtime.roomName,
          showtimes: [],
        };
      }
      const date = new Date(showtime.startTime).toISOString();
      const timePart = date.includes("T")
        ? date.split("T")[1]?.slice(0, 5)
        : "00:00";
      acc[showtime.roomId].showtimes.push(timePart);
      return acc;
    }, {});
    setGroupedShowtimes(grouped);
  }, [listRoom]);

  return (
    <div className={`detail-container ${darkMode ? "dark" : "light"}`}>
      <h2 className="title">
        <span className="movie-title-description">LỊCH CHIẾU</span>
      </h2>

      {showtimes.length > 0 ? (
        <div className="date-picker">
          {(() => {
            const uniqueDates = new Map();
            const filteredShowtimes = showtimes.filter((showtimeItem) => {
              const startTime = showtimeItem.startTime;
              const formattedDate = new Date(startTime).toLocaleDateString(
                "vi-VN"
              );

              if (uniqueDates.has(formattedDate)) {
                return false;
              }

              uniqueDates.set(formattedDate, showtimeItem.showtimeId);
              return true;
            });

            return filteredShowtimes.slice(0, 7).map((showtimeItem) => {
              const startTime = showtimeItem.startTime;
              const dateObj = new Date(startTime);
              const formattedDate = dateObj.toLocaleDateString("vi-VN", {
                day: "2-digit",
                month: "2-digit",
              });
              const dayOfWeek = dateObj.toLocaleDateString("vi-VN", {
                weekday: "long",
              });

              return (
                <Button
                  size="large"
                  key={showtimeItem.showtimeId}
                  className={`date-btn ${
                    selectedShowtimeDate === startTime ? "active" : ""
                  }`}
                  onClick={() => setSelectedShowtimeDate(startTime)}
                >
                  <span>
                    {formattedDate}
                    <br />
                    {dayOfWeek}
                  </span>
                </Button>
              );
            });
          })()}
        </div>
      ) : (
        <p className="no-movie">Chưa có lịch chiếu.</p>
      )}
      {groupedShowtimes && (
        <>
          <h3 className="subtitle">
            <span className="movie-title-description">DANH SÁCH PHÒNG</span>
          </h3>

          {Object.keys(groupedShowtimes).length > 0 ? (
            <Collapse className="cinema-list" accordion>
              {Object.entries(groupedShowtimes).map(
                ([roomId, { roomName, showtimes }]) => (
                  <Panel
                    header={<span className="cinema-title">{roomName}</span>}
                    key={roomId}
                    className="cinema-panel"
                  >
                    <div className="showtime-buttons">
                      {showtimes.map((time: string, index: number) => (
                        <Tag
                          key={index}
                          onClick={() => setSelectedRoomId(Number(roomId))}
                          className="showtime"
                        >
                          {time}
                        </Tag>
                      ))}
                    </div>
                  </Panel>
                )
              )}
            </Collapse>
          ) : (
            <p>Vui lòng chọn ngày chiếu phim.</p>
          )}
        </>
      )}
      {selectedRoomId ? (
        <Seat RoomId={selectedRoomId} onSelectSeat={listSeats} />
      ) : null}

      {selectedRoomId ? <FoodCombo /> : null}
    </div>
  );
};

export default ShowtimeSchedule;
