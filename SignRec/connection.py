import socket
import os
import numpy as np
import cv2
import struct


HOST = '172.31.23.175'  # Listen on all network interfaces
PORT = 1114

def save_image(data):
    # Create a unique filename for the image
    filename = 'received_image.jpg'

    # Save the image data to a file
    with open(filename, 'wb') as f:
        f.write(data)

    print('Image saved as', filename)

def start_server():
    # Create a TCP/IP socket
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # Bind the socket to the host and port
    server_socket.bind((HOST, PORT))

    # Listen for incoming connections
    server_socket.listen(1)

    print('Server listening on {}:{}'.format(HOST, PORT))

    # Accept a client connection
    client_socket, client_address = server_socket.accept()
    print('Client connected:', client_address)

    # Receive the size of the incoming byte array
    size_bytes = client_socket.recv(4)
    array_size = struct.unpack('i', size_bytes)[0]
    print("array size ", array_size)
    
    # Receive the byte array
    data = bytearray()
    while len(data) < array_size:
        chunk = client_socket.recv(array_size - len(data))
        if not chunk:
            break
        data.extend(chunk)

    print(f"Received byte array with size: {array_size}")

    # Convert the byte string to a numpy array
    image_array = np.frombuffer(data, dtype=np.uint8)

    # Decode the image array using OpenCV
    image = cv2.imdecode(image_array, cv2.IMREAD_COLOR)

    if image is None:
        print("Failed to decode the image data")
    else:
        # Save the image as a file using OpenCV
        cv2.imwrite('received_image2.jpg', image)
        print("Image saved successfully")
    
    # Close the client socket
    client_socket.close()

start_server()
