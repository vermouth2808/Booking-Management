import React from "react";
import { Card, Button, Typography, Row, Col } from "antd";
import "./TotalMovieTicketPrice.css";

const { Title, Text } = Typography;
const TotalMovieTicketPrice = () => {
  return (
    <Card className="TotalMovieTicketPrice">
      <Row justify="space-between" align="middle">
        {/* Thông tin vé */}
        <Col>
          <Title level={4} className="movie-title"></Title>
          <Text className="ticket-type">Người Lớn</Text>
        </Col>

        {/* Tổng giá vé */}
        <Col style={{ textAlign: "right" }}>
          <Text className="ticket-price-text">Tạm tính</Text>
          <Title level={3} className="ticket-price"></Title>
        </Col>

        {/* Nút đặt vé */}
        <Col>
          <Button type="primary" className="booking-button">
            ĐẶT VÉ
          </Button>
        </Col>
      </Row>
    </Card>
  );
};

export default TotalMovieTicketPrice;
