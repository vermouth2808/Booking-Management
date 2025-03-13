import React, { useEffect, useState } from "react";
import { Collapse, Tag, Button } from "antd";
import { useSelector } from "react-redux";
import ShowTimeService from "../../services/ShowTimeService";
import "./ShowtimeSchedule.css";

const { Panel } = Collapse;

const ShowtimeSchedule = ({ movieId }) => {
  const darkMode = useSelector((state) => state.theme.darkMode);
  const [showtimes, setShowtimes] = useState([]);
  const [showtimeDetail, setShowtimeDetail] = useState([]);
  const [selectedShowtimeId, setSelectedShowtimeId] = useState(null);

  useEffect(() => {
    const fetchShowTime = async () => {
      try {
        const now = new Date();
        const params = {
          keySearch: "",
          pageSize: 10,
          pageIndex: 1,
          fromDate: new Date(now.getFullYear(), now.getMonth(), 1).toISOString(),
          toDate: new Date(now.getFullYear(), now.getMonth() + 1, 0).toISOString(),
          movieId: parseInt(movieId, 10),
          roomId: null,
        };
        const showtime = await ShowTimeService.SearchShowTime(params);
        setShowtimes(Array.isArray(showtime) ? showtime : []);
      } catch (error) {
        console.log("Load showtimes failed", error);
      }
    };
    fetchShowTime();
  }, [movieId]);

  useEffect(() => {
    const fetchGetDetailShowtime = async () => {
      if (!selectedShowtimeId) return;
      try {
        const detailShowtime = await ShowTimeService.GetDetailShowTime(selectedShowtimeId);
        setShowtimeDetail(Array.isArray(detailShowtime) ? detailShowtime : []);
      } catch (error) {
        console.log("Load showtime detail failed", error);
      }
    };
    fetchGetDetailShowtime();
  }, [selectedShowtimeId]);

  return (
    <div className={`detail-container ${darkMode ? "dark" : "light"}`}>
      <h2 className="title">
        <span className="movie-title">LỊCH CHIẾU</span>
      </h2>

      {/* Hiển thị danh sách ngày chiếu (không trùng lặp) */}
      {showtimes.length > 0 ? (
        <div className="date-picker">
          {(() => {
            const uniqueDates = new Map();

            return showtimes.map((showtimeItem) => {
              const startTime = showtimeItem.startTime;
              const dateObj = new Date(startTime);
              const formattedDate = dateObj.toLocaleDateString("vi-VN");
              const dayOfWeek = dateObj.toLocaleDateString("vi-VN", { weekday: "long" });

              // Kiểm tra nếu ngày đã có trong Map thì bỏ qua
              if (uniqueDates.has(formattedDate)) {
                return null;
              }

              uniqueDates.set(formattedDate, showtimeItem.showtimeId);

              return (
                <Button
                  size="large"
                  key={showtimeItem.showtimeId}
                  className={`date-btn ${selectedShowtimeId === showtimeItem.showtimeId ? "active" : ""}`}
                  onClick={() => setSelectedShowtimeId(showtimeItem.showtimeId)}
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

      <h3 className="subtitle">
        <span className="movie-title">DANH SÁCH PHÒNG</span>
      </h3>

      {/* Danh sách phòng chiếu */}
      {showtimeDetail.length > 0 ? (
        <Collapse className="cinema-list" accordion>
          {showtimeDetail.map((showtime) => (
            <Panel
              header={<span className="cinema-title">{showtime.title}</span>}
              key={showtime.showtimeId}
              className="cinema-panel"
            >
              <p className="cinema-address">{showtime.roomName}</p>
              <div className="showtime-buttons">
                <Tag className="showtime">{showtime.startTime}</Tag>
              </div>
            </Panel>
          ))}
        </Collapse>
      ) : (
        <p>Vui lòng chọn ngày chiếu phim.</p>
      )}
    </div>
  );
};

export default ShowtimeSchedule;
