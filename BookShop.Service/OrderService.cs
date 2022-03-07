using BookShop.Data.Infrastructure;
using BookShop.Data.Repositories;
using BookShop.Model.Models;
using System;
using System.Collections.Generic;

namespace BookShop.Service
{
    public interface IOrderService
    {
        
        Order Create(ref Order order, List<OrderDetail> orderDetails);
        void UpdateStatus(int orderId);
        void UpdateShipmentId(int orderId, string shipmentId);
        void Save();
        IEnumerable<OrderInformation> GetAllOrderInformation();
        IEnumerable<OrderInformation> GetAllOrderInformationByOrderId(int? orderId);
        OrderInformation GetOrderInformationByOrderId(int orderId);
        IEnumerable<OrderInformation> GetOrdersInformationByUserId(string userId);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }
        public Order Create(ref Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
            _unitOfWork.Commit();
        }
        public void UpdateShipmentId(int orderId, string shipmentId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.ShipmentId = shipmentId;
            _orderRepository.Update(order);
            _unitOfWork.Commit();
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<OrderInformation> GetAllOrderInformation()
        {
           return _orderRepository.GetAllOrderInformation();
        }

        public IEnumerable<OrderInformation> GetAllOrderInformationByOrderId(int? orderId)
        {
            if (orderId != null)
            {
                return _orderRepository.GetAllOrderInformationByKeyword(orderId);
            }
            return _orderRepository.GetAllOrderInformation();
        }

        public OrderInformation GetOrderInformationByOrderId(int orderId)
        {
            return _orderRepository.GetOrderInformationByOrderId(orderId);
        }

        public IEnumerable<OrderInformation> GetOrdersInformationByUserId(string userId)
        {
            return _orderRepository.GetOrdersInformationByUserId(userId);
        }
    }
}
