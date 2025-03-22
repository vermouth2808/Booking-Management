import React, { useEffect, useState } from "react";
import "./Seat.css";
import { Button } from "antd";
import RoomService from "../../services/RoomService";

interface RoomDetail {
  roomName: string;
  layout: {
    seats: (string | null)[][]; 
  };
}

interface SeatProps {
  RoomId: number | null;
  onSelectSeat: (selectedSeats: string[]) => void;
}

const Seat: React.FC<SeatProps> = ({ RoomId, onSelectSeat }) => {
  const [roomDetail, setRoomDetail] = useState<RoomDetail | null>(null);
  const [listSeat, setListSeat] = useState<string[]>([]);
  useEffect(() => {
    onSelectSeat(listSeat);
    return () => {
      onSelectSeat([]);
    };
  }, [listSeat, onSelectSeat]);
  


  const handleSelectSeat = (seat: string) => {
    setListSeat((prev) =>
      prev.includes(seat) ? prev.filter((s) => s !== seat) : [...prev, seat]
    );
  };

  useEffect(() => {
    const fetchGetDetailRoom = async () => {
      try {
        const room = await RoomService.getDetailRoom(RoomId);

        if (room && typeof room.layout === "string") {
          room.layout = JSON.parse(room.layout);
        }
        setRoomDetail(room);
      } catch (error) {
        console.error("Load fetchGetDetailRoom failed", error);
      }
    };

    if (RoomId !== null) {
      fetchGetDetailRoom();
    }
  }, [RoomId]);

  return (
    <div className="seat-wrapper">
      {roomDetail && (
        <div className="title">
          <span>{roomDetail.roomName}</span>
        </div>
      )}

      <div className="screen">MÀN HÌNH</div>

      <div className="seat-container">
        {roomDetail?.layout?.seats ? (
          roomDetail.layout.seats.map((row, rowIndex) => (
            <div key={rowIndex} className="seat-row">
              {row.map((seat, colIndex) =>
                seat ? (
                  <Button
                    key={seat}
                    className={`seat-btn ${
                      listSeat.includes(seat) ? "selected" : ""
                    }`}
                    onClick={() => handleSelectSeat(seat)}
                  >
                    {seat}
                  </Button>
                ) : (
                  <div key={colIndex} className="seat-empty"></div>
                )
              )}
            </div>
          ))
        ) : (
          <p>Vui lòng chọn khung giờ chiếu phim.</p>
        )}
      </div>

      {/* Chú thích màu ghế */}
      <div className="seat-legend">
        <div className="legend-item">
          <div className="seat-btn"></div>
          <span>Ghế trống</span>
        </div>
        <div className="legend-item">
          <div className="seat-btn vip"></div>
          <span>Ghế VIP</span>
        </div>
        <div className="legend-item">
          <div className="seat-btn booked"></div>
          <span>Ghế đã đặt</span>
        </div>
        <div className="legend-item">
          <div className="seat-btn selected"></div>
          <span>Ghế đã chọn</span>
        </div>
      </div>
    </div>
  );
};

export default Seat;
