import React, { useState,useEffect } from "react";
import { Card, Button, Typography } from "antd";
import { useSelector } from "react-redux";
import { MinusOutlined, PlusOutlined } from "@ant-design/icons";
import "antd/dist/reset.css";
import "./FoodCombo.css";
import FoodComboService from "../../services/FoodComboService";
import { ReadAllFoodComboRes } from "../../models/ReadAllFoodComboRes";

const { Title, Text } = Typography;



const FoodCombo = () => {
  const [quantities, setQuantities] = useState<Record<number, number>>({ 1: 0, 2: 0 });
  const darkMode = useSelector((state: any) => state.theme.darkMode);
  const [foodCombo, setFoodCombo] = useState<ReadAllFoodComboRes[]>([]);

  useEffect( ()=>{

    const fetchFoodCombo = async () => {
     const combos : ReadAllFoodComboRes[] =  await FoodComboService.getAllFoodCombo();
     setFoodCombo(combos);
    };
    
    fetchFoodCombo()
    },[]);

  const updateQuantity = (id: number, delta: number) => {
    setQuantities((prev) => ({
      ...prev,
      [id]: Math.max(0, (prev[id] || 0) + delta),
    }));
  };

  return (
    <div className={`cinema-container ${darkMode ? "dark" : ""}`}>
      <Title level={1}>
        <span className="food-combo-title">CHỌN COMBO BẮP NƯỚC</span>
      </Title>
      <div className="combo-list">
        {foodCombo.map((combo) => (
          <Card
            key={combo.comboId}
            className="combo-card"
            cover={
              <img
                src={combo.imageUrl}
                alt={combo.name}
                className="combo-image"
              />
            }
          >
            <Title level={4}>
              <span className="combo-name">{combo.name}</span>
            </Title>
            <Text>
              <span className="combo-description">{combo.description}</span>
            </Text>
            <Text>
              <span className="combo-price">{combo.price}.000đ</span>
           </Text>
            <div className="quantity-controls">
              <Button
                shape="circle"
                icon={<MinusOutlined />}
                onClick={() => updateQuantity(combo.comboId, -1)}
                className="quantity-button"
              />
              <span className="quantity-text">{quantities[combo.comboId]}</span>
              <Button
                shape="circle"
                icon={<PlusOutlined />}
                onClick={() => updateQuantity(combo.comboId, 1)}
                className="quantity-button"
              />
            </div>
          </Card>
        ))}
      </div>
    </div>
  );
};

export default FoodCombo;