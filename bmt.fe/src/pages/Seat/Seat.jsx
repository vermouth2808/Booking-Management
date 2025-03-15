import React, { useEffect, useState } from "react";
import "../Seat/Seat.css";
import { Button } from "antd";
import RoomService from "../../services/RoomService";

const Seat = ({ RoomId }) => {
    const [roomDetail, setRoomDetail] = useState(null);

    useEffect(() => {
        const fetchGetDetailRoom = async () => {
            try {
                const room = await RoomService.getDetailRoom(RoomId);

                console.log("room:", room);
                console.log("layout:", room.layout);

                if (room && typeof room.layout === "string") {
                    room.layout = JSON.parse(room.layout);
                }
                setRoomDetail(room);
            } catch (error) {
                console.log("Load fetchGetDetailRoom failed", error);
            }
        };
        fetchGetDetailRoom();
    }, [RoomId]);

    return (
        <div className="seat-wrapper">
            <div className="screen">MÀN HÌNH</div>
            <div className="seat-container">
                {roomDetail && roomDetail.layout && roomDetail.layout.seats ? (
                    roomDetail.layout.seats.map((row, rowIndex) => (
                        <div key={rowIndex} className="seat-row">
                            {row.map((seat, colIndex) =>
                                seat ? (
                                    <Button key={seat} className="seat-btn">
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
